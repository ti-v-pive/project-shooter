using System;
using UniRx;

namespace Utils {
    public class Signal : IObservable<Unit> {
        private readonly Subject<Unit> _signalStream = new Subject<Unit>();
        public void Fire() { _signalStream.OnNext(Unit.Default); }
        public IDisposable Subscribe(IObserver<Unit> observer) { return _signalStream.Subscribe(observer.OnNext); }
    }

    public class Signal<T> : IObservable<T> {
        private readonly Subject<T> _signalStream = new Subject<T>();
        public void Fire(T data) { _signalStream.OnNext(data); }
        public IDisposable Subscribe(IObserver<T> observer) { return _signalStream.Subscribe(observer.OnNext); }
    }
}