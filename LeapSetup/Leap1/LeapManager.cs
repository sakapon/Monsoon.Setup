﻿using System;
using System.Diagnostics;
using System.Reactive.Subjects;
using Leap;
using Reactive.Bindings;

namespace Monsoon.Setup.Leap
{
    [DebuggerDisplay("IsDisposed: {_isDisposed}")]
    public class LeapManager : IDisposable
    {
        Subject<Frame> _FrameArrived = new Subject<Frame>();
        public IObservable<Frame> FrameArrived => _FrameArrived;

        ReactiveProperty<bool> _IsConnected = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IsConnected => _IsConnected;

        public Controller Controller { get; }
        FrameListener _listener;
        bool _isDisposed;

        public LeapManager()
        {
            Controller = new Controller();
            _listener = new FrameListener(this);
            Controller.AddListener(_listener);

            Controller.SetPolicyFlags(Controller.PolicyFlag.POLICY_BACKGROUND_FRAMES);
        }

        ~LeapManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;
            _isDisposed = true;

            // ファイナライザー (disposing == false のとき) では、
            // 既にここで Controller および Listener が dispose されており、
            // RemoveListener メソッドを呼び出すと例外が発生します。
            if (disposing)
            {
                Controller.RemoveListener(_listener);
                _listener.Dispose();
                Controller.Dispose();
            }
        }

        class FrameListener : Listener
        {
            public LeapManager Manager { get; }

            public FrameListener(LeapManager manager)
            {
                Manager = manager;
            }

            public override void OnFrame(Controller controller)
            {
                if (!Manager._FrameArrived.HasObservers) return;

                using (var frame = controller.Frame())
                {
                    Manager._FrameArrived.OnNext(frame);
                }
            }

            public override void OnConnect(Controller controller)
            {
                Manager._IsConnected.Value = controller.IsConnected;
            }

            public override void OnDisconnect(Controller controller)
            {
                Manager._IsConnected.Value = controller.IsConnected;
            }
        }
    }
}
