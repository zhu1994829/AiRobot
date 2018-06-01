using MX.AIRobot.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MX.AIRobot.AOP
{
    /// <summary>
    /// AspectF
    /// (C) Omar AL Zabir 2009 All rights reserved.
    /// 
    /// AspectF lets you add strongly typed Aspects within you code, 
    /// anywhere in the code, in a fluent way. In common AOP frameworks, 
    /// you define aspects as individual classes and you leave indication 
    /// in the code where the aspect needs to be injected. A weaver 
    /// then weaves it into the code for you. You can also implement AOP
    /// using Attributes and by inheriting your classes from MarshanByRef. 
    /// But that's not an option for you always to do so. There's also 
    /// another way of doing AOP using DynamicProxy.
    /// 
    /// AspectF tries to avoid all these special tricks. It has no need 
    /// for a weaver (or any post build tool). It also does not require
    /// extending classes from MarshalByRef or using DynamicProxy.
    /// 
    /// AspectF offers a plain vanilla way of putting aspects within 
    /// your methods. You can wrap your code using Aspects 
    /// by using standard wellknown C#/VB.NET code. 
    /// </summary>
    public class AspectF
    {
        /// <summary>
        /// Chain of aspects to invoke
        /// </summary>

        internal Action<Action> Chain = null;

        /// <summary>
        /// The acrual work delegate that is finally called
        /// </summary>
        internal Delegate WorkDelegate;

        /// <summary>
        /// Create a composition of function e.g. f(g(x))
        /// </summary>
        /// <param name="newAspectDelegate">A delegate that offers an aspect's behavior. 
        /// It's added into the aspect chain</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public AspectF Combine(Action<Action> newAspectDelegate)
        {
            if (this.Chain == null)
            {
                this.Chain = newAspectDelegate;
            }
            else
            {
                Action<Action> existingChain = this.Chain;
                Action<Action> callAnother = (work) => existingChain(() => newAspectDelegate(work));
                this.Chain = callAnother;
            }
            return this;
        }

        /// <summary>
        /// Execute your real code applying the aspects over it
        /// </summary>
        /// <param name="work">The actual code that needs to be run</param>
        [DebuggerStepThrough]
        public void Do(Action work)
        {
            if (this.Chain == null)
            {
                work();
            }
            else
            {
                this.Chain(work);
            }
        }

        /// <summary>
        /// Execute your real code applying aspects over it.
        /// </summary>
        /// <typeparam name="TReturnType"></typeparam>
        /// <param name="work">The actual code that needs to be run</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public TReturnType Return<TReturnType>(Func<TReturnType> work)
        {
            this.WorkDelegate = work;

            if (this.Chain == null)
            {
                return work();
            }
            else
            {
                TReturnType returnValue = default(TReturnType);
                this.Chain(() =>
                {
                    Func<TReturnType> workDelegate = WorkDelegate as Func<TReturnType>;
                    returnValue = workDelegate();
                });
                return returnValue;
            }
        }

        /// <summary>
        /// Handy property to start writing aspects using fluent style
        /// </summary>
        public static AspectF Define
        {
            [DebuggerStepThrough]
            get
            {
                return new AspectF();
            }
        }
    }

    public static class AspectExtensions
    {
        [DebuggerStepThrough]
        public static void DoNothing()
        {
        }

        [DebuggerStepThrough]
        public static void DoNothing(params object[] whatever)
        {
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, ILogger logger)
        {
            return aspects.Combine((work) =>
                Retry(1000, 1, (error) => DoNothing(error), x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, Action<IEnumerable<Exception>> failHandler, ILogger logger)
        {
            return aspects.Combine((work) =>
                Retry(1000, 1, (error) => DoNothing(error), x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration, ILogger logger)
        {
            return aspects.Combine((work) =>
                Retry(retryDuration, 1, (error) => DoNothing(error), x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration,
            Action<Exception> errorHandler, ILogger logger)
        {
            return aspects.Combine((work) =>
                Retry(retryDuration, 1, errorHandler, x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration,
            int retryCount, Action<Exception> errorHandler, ILogger logger)
        {
            return aspects.Combine((work) =>
                Retry(retryDuration, retryCount, errorHandler, x => DoNothing(), work, logger));
        }

        [DebuggerStepThrough]
        public static AspectF Retry(this AspectF aspects, int retryDuration,
            int retryCount, Action<Exception> errorHandler, Action<IEnumerable<Exception>> retryFailed, ILogger logger)
        {
            return aspects.Combine((work) =>
                Retry(retryDuration, retryCount, errorHandler, retryFailed, work, logger));
        }

        [DebuggerStepThrough]
        public static void Retry(int retryDuration, int retryCount,
            Action<Exception> errorHandler, Action<IEnumerable<Exception>> retryFailed, Action work, ILogger logger)
        {
            List<Exception> errors = null;
            do
            {
                try
                {
                    work();
                    return;
                }
                catch (Exception x)
                {
                    if (null == errors)
                        errors = new List<Exception>();
                    errors.Add(x);
                    logger.WriteLog(LogLevel.Error, x);
                    errorHandler(x);
                    System.Threading.Thread.Sleep(retryDuration);
                    throw x;
                }
            } while (retryCount-- > 0);
            retryFailed(errors);
        }

        [DebuggerStepThrough]
        public static AspectF Delay(this AspectF aspect, int milliseconds)
        {
            return aspect.Combine((work) =>
            {
                System.Threading.Thread.Sleep(milliseconds);
                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF MustBeNonDefault<T>(this AspectF aspect, params T[] args)
            where T : IComparable
        {
            return aspect.Combine((work) =>
            {
                T defaultvalue = default(T);
                for (int i = 0; i < args.Length; i++)
                {
                    T arg = args[i];
                    if (arg == null || arg.Equals(defaultvalue))
                        throw new ArgumentException(
                            string.Format("Parameter at index {0} is null", i));
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF MustBeNonNull(this AspectF aspect, params object[] args)
        {
            return aspect.Combine((work) =>
            {
                for (int i = 0; i < args.Length; i++)
                {
                    object arg = args[i];
                    if (arg == null)
                        throw new ArgumentException(
                            string.Format("Parameter at index {0} is null", i));
                }

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF Until(this AspectF aspect, Func<bool> test)
        {
            return aspect.Combine((work) =>
            {
                while (!test()) ;

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF While(this AspectF aspect, Func<bool> test)
        {
            return aspect.Combine((work) =>
            {
                while (test())
                    work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF WhenTrue(this AspectF aspect, params Func<bool>[] conditions)
        {
            return aspect.Combine((work) =>
            {
                foreach (Func<bool> condition in conditions)
                    if (!condition())
                        return;

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, ILogger logger, string[] categories,
            string logMessage, params object[] arg)
        {
            return aspect.Combine((work) =>
            {
                logger.WriteLog(LogLevel.Info, categories, logMessage);

                work();
            });
        }

        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, ILogger logger,
            string logMessage, params object[] arg)
        {
            return aspect.Combine((work) =>
            {
                logger.WriteLog(LogLevel.Info, string.Format("■" + logMessage, arg));

                work();
            });
        }


        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, ILogger logger, string[] categories,
            string beforeMessage, string afterMessage)
        {
            return aspect.Combine((work) =>
            {
                logger.WriteLog(LogLevel.Info, categories, "■" + beforeMessage);

                work();

                logger.WriteLog(LogLevel.Info, categories, "■" + afterMessage);
            });
        }

        [DebuggerStepThrough]
        public static AspectF Log(this AspectF aspect, ILogger logger,
            string beforeMessage, string afterMessage)
        {
            return aspect.Combine((work) =>
            {
                logger.WriteLog(LogLevel.Info, "■" + beforeMessage + "▼▼▼▼▼ ThreadID:" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

                work();

                logger.WriteLog(LogLevel.Info, "■" + afterMessage + "▲▲▲▲▲ ThreadID:" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            });
        }

        [DebuggerStepThrough]
        public static AspectF HowLong(this AspectF aspect, ILogger logger,
            string invokerMessage = "", string startMessage = " 开始记时", string endMessage = " 结束记时 ★消耗时间为:{0}ms,{1}s,{2}M,{3}H★")
        {
            return aspect.Combine((work) =>
            {
                DateTime start = DateTime.Now.ToUniversalTime();
                logger.WriteLog(LogLevel.Info, "■" + invokerMessage + startMessage + "ThreadID:" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

                work();

                DateTime end = DateTime.Now.ToUniversalTime();
                TimeSpan duration = end - start;

                logger.WriteLog(LogLevel.Info, "■" + invokerMessage + string.Format(endMessage, duration.TotalMilliseconds,
                    duration.TotalSeconds, duration.TotalMinutes, duration.TotalHours,
                    duration.TotalDays) + "ThreadID:" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            });
        }

        [DebuggerStepThrough]
        public static AspectF VerifyPermission(this AspectF aspect, ILogger logger, IAuth auth, Action AuthErrorHandler)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    if (auth.VerifyPermission())
                    {
                        work();
                    }
                    else
                    {
                        AuthErrorHandler();
                    }
                }
                catch (Exception x)
                {
                    logger.WriteLog(LogLevel.Error, x);
                    throw;
                }
            });
        }

        [DebuggerStepThrough]
        public static AspectF VerifyPermission(this AspectF aspect, ILogger logger, IAuth auth, Action AuthHandler, Action AuthErrorHandler)
        {
            return aspect.Combine((work) =>
            {
                try
                {

                    if (auth.VerifyPermission())
                    {
                        AuthHandler();
                        work();
                    }
                    else
                    {
                        AuthErrorHandler();
                    }
                }
                catch (Exception x)
                {
                    logger.WriteLog(LogLevel.Error, x);
                    throw x;
                }
            });
        }


        [DebuggerStepThrough]
        public static AspectF TrapLog(this AspectF aspect, ILogger logger)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    work();
                }
                catch (Exception x)
                {
                    logger.WriteLog(LogLevel.Error, x);
                }
            });
        }

        [DebuggerStepThrough]
        public static AspectF TrapLogThrow(this AspectF aspect, ILogger logger)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    work();
                }
                catch (Exception x)
                {
                    logger.WriteLog(LogLevel.Error, x);
                    throw;
                }
            });
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect, Action completeCallback)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncresult =>
            {
                work.EndInvoke(asyncresult); completeCallback();
            }, null));
        }

        [DebuggerStepThrough]
        public static AspectF RunAsync(this AspectF aspect)
        {
            return aspect.Combine((work) => work.BeginInvoke(asyncresult =>
            {
                work.EndInvoke(asyncresult);
            }, null));
        }

        [DebuggerStepThrough]
        public static AspectF Cache<TReturnType>(this AspectF aspect,
            ICache cacheResolver, string key)
        {
            return aspect.Combine((work) =>
            {
                Cache<TReturnType>(aspect, cacheResolver, key, work, cached => cached);
            });
        }

        [DebuggerStepThrough]
        public static AspectF CacheList<TItemType, TListType>(this AspectF aspect,
            ICache cacheResolver, string listCacheKey, Func<TItemType, string> getItemKey)
            where TListType : IList<TItemType>, new()
        {
            return aspect.Combine((work) =>
            {
                Func<TListType> workDelegate = aspect.WorkDelegate as Func<TListType>;

                // Replace the actual work delegate with a new delegate so that
                // when the actual work delegate returns a collection, each item
                // in the collection is stored in cache individually.
                Func<TListType> newWorkDelegate = () =>
                {
                    TListType collection = workDelegate();
                    foreach (TItemType item in collection)
                    {
                        string key = getItemKey(item);
                        cacheResolver.Set(key, item);
                    }
                    return collection;
                };
                aspect.WorkDelegate = newWorkDelegate;

                // Get the collection from cache or real source. If collection is returned
                // from cache, resolve each item in the collection from cache
                Cache<TListType>(aspect, cacheResolver, listCacheKey, work,
                    cached =>
                    {
                        // Get each item from cache. If any of the item is not in cache
                        // then discard the whole collection from cache and reload the 
                        // collection from source.
                        TListType itemList = new TListType();
                        foreach (TItemType item in cached)
                        {
                            object cachedItem = cacheResolver.Get(getItemKey(item));
                            if (null != cachedItem)
                            {
                                itemList.Add((TItemType)cachedItem);
                            }
                            else
                            {
                                // One of the item is missing from cache. So, discard the 
                                // cached list.
                                return default(TListType);
                            }
                        }

                        return itemList;
                    });
            });
        }

        [DebuggerStepThrough]
        public static AspectF CacheRetry<TReturnType>(this AspectF aspect,
            ICache cacheResolver,
            ILogger logger,
            string key)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    Cache<TReturnType>(aspect, cacheResolver, key, work, cached => cached);
                }
                catch (Exception x)
                {
                    logger.WriteLog(LogLevel.Error, x);
                    System.Threading.Thread.Sleep(1000);

                    //Retry
                    try
                    {
                        Cache<TReturnType>(aspect, cacheResolver, key, work, cached => cached);
                    }
                    catch (Exception ex)
                    {
                        logger.WriteLog(LogLevel.Error, ex);
                        throw ex;
                    }
                }
            });
        }

        private static void Cache<TReturnType>(AspectF aspect, ICache cacheResolver,
            string key, Action work, Func<TReturnType, TReturnType> foundInCache)
        {
            object cachedData = cacheResolver.Get(key);
            if (cachedData == null)
            {
                GetListFromSource<TReturnType>(aspect, cacheResolver, key);
            }
            else
            {
                // Give caller a chance to shape the cached item before it is returned
                TReturnType cachedType = foundInCache((TReturnType)cachedData);
                if (cachedType == null)
                {
                    GetListFromSource<TReturnType>(aspect, cacheResolver, key);
                }
                else
                {
                    aspect.WorkDelegate = new Func<TReturnType>(() => cachedType);
                }
            }

            work();
        }

        private static void GetListFromSource<TReturnType>(AspectF aspect, ICache cacheResolver, string key)
        {
            Func<TReturnType> workDelegate = aspect.WorkDelegate as Func<TReturnType>;
            TReturnType realObject = workDelegate();
            cacheResolver.Add(key, realObject);
            workDelegate = () => realObject;
            aspect.WorkDelegate = workDelegate;
        }


    }
}
