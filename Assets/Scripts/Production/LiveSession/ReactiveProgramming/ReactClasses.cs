using System;
using System.Collections.Generic;

namespace Tools
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext, bool notifyOnSubscribe = true)
        {
            IObserver<T> observer = new ActionToObserver<T>(onNext);
            return observable.Subscribe(observer, notifyOnSubscribe);
        }

        public static IDisposable Subscribe<T>(this IObservable<T> observable, IObserver<T> observer, bool notifyOnSubscribe = true)
        {
            if (notifyOnSubscribe)
            {
                return observable.Subscribe(observer);
            }
            else
            {
                return observable.Subscribe(new SkipFirstNotificationObserver<T>(observer));
            }
        }
    }

    public class SkipFirstNotificationObserver<T> : IObserver<T>
    {
        private bool m_IsFirstTime = true;
        private readonly IObserver<T> m_InnerObserver;
        public SkipFirstNotificationObserver(IObserver<T> innerObserver)
        {
            m_InnerObserver = innerObserver;
        }
        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(T value)
        {
            if (m_IsFirstTime)
            {
                m_IsFirstTime = false;
            }
            else
            {
                m_InnerObserver.OnNext(value);
            }
        }
    }

    public class ActionToObserver<T> : IObserver<T>
    {
        private readonly Action<T> m_Action;

        public ActionToObserver(Action<T> action)
        {
            m_Action = action;
        }

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(T value)
        {
            m_Action.Invoke(value);
        }
    }

    public class ObservableProperty<T> : IObservable<T>
    {
        private bool m_IsInitialised;
        private T m_Value;
        private readonly Subject<T> m_Subject = new Subject<T>();

        public T Value
        {
            get => m_Value;
            set
            {
                m_IsInitialised = true;
                if (EqualityComparer<T>.Default.Equals(m_Value, value) == false)
                {
                    Value = m_Value;
                    m_Subject.OnNext(m_Value);
                }
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            IDisposable subscription = null;
            try
            {
                if(m_IsInitialised)
                {
                    observer.OnNext(m_Value);
                }
            }
            finally
            {
                subscription = m_Subject.Subscribe(observer);
            }
            return subscription;
        }
    }

    public interface ISubject<T> : IObservable<T>, IObserver<T> { }

    public class Subject<T> : ISubject<T>
    {
        private readonly List<IObserver<T>> m_Observers =  new List<IObserver<T>>();
        
        public void OnCompleted()
        {
            for (int i = 0; i < m_Observers.Count; i++)
            {
                m_Observers[i].OnCompleted();
            }
        }

        public void OnError(Exception error)
        {
            for (int i = 0; i < m_Observers.Count; i++)
            {
                m_Observers[i].OnError(error);
            }
        }

        public void OnNext(T value)
        {
            for (int i = 0; i < m_Observers.Count; i++)
            {
                m_Observers[i].OnNext(value);
            }
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            m_Observers.Add(observer);
            return new Subscription(m_Observers, observer);
        }

        private class Subscription : IDisposable
        {
            private readonly List<IObserver<T>> m_Observers;
            private readonly IObserver<T> m_Observer;

            public Subscription(List<IObserver<T>> observers, IObserver<T> observer)
            {
                m_Observers = observers;
                m_Observer = observer;
            }

            public void Dispose()
            {
                if(m_Observers.Contains(m_Observer))
                {
                    m_Observers.Remove(m_Observer);
                }
            }
        }
    }
    public class SubjectCaller
    {
        public void SubscribeToSubject()
        {
            Subject<int> intStream = new Subject<int>();
            IDisposable subscription = intStream.Subscribe(null);
            subscription.Dispose();
        }
    }
}