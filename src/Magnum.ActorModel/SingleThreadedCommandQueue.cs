namespace Magnum.ActorModel
{
	using System;
	using System.Threading;

	public class SingleThreadedCommandQueue :
		IDisposable,
		IStartable,
		CommandQueue
	{
		private readonly CommandQueue _queue;
		private readonly Thread _thread;

		public SingleThreadedCommandQueue(CommandQueue queue)
		{
			_queue = queue;
			_thread = new Thread(RunThread);
			_thread.Name = string.Format("SingleThreadedCommandQueue-{0}", _thread.ManagedThreadId);
			_thread.IsBackground = false;
			_thread.Priority = ThreadPriority.Normal;
		}

		public Thread Thread
		{
			get { return _thread; }
		}

		public void Dispose()
		{
			_queue.Disable();
		}

		private void RunThread()
		{
			try
			{
				_queue.Run();
			}
			catch (Exception)
			{
				//TODO
			}
		}

		public void Join()
		{
			_thread.Join();
		}

		public void Enqueue(Action action)
		{
			_queue.Enqueue(action);
		}

		public void EnqueueAll(params Action[] actions)
		{
			_queue.EnqueueAll(actions);
		}

		public void Disable()
		{
			_queue.Disable();
		}

		public void Run()
		{
		}

		public void Start()
		{
			_thread.Start();
		}
	}

	public interface IStartable
	{
		void Start();
	}
}