using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DuoSoftware.DuoTools.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    public sealed class SequenceNumberGenerator
    {

        #region Local Variables

        private static volatile SequenceNumberGenerator _instance;
        private static readonly object SyncRoot = new Object();
        
        private SortedList<int,int> noList;
        private int end = 0;
        private bool IsLoading = false;
        #endregion

        #region Properties


        #endregion

        #region Constructors

        private SequenceNumberGenerator()
        {
            try
            {
                noList = new SortedList<int, int>(30000);
                GenerateNos();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SequenceNumberGenerator", exception, Logger.LogLevel.Error);
                throw;
            }
        }



        #endregion

        #region Interface Implementations


        #endregion

        #region Events

        #endregion

        #region Methods

        #region Internal Methods

        #endregion

        #region Private Methods

        private void GenerateNos()
        {
            try
            {
                IsLoading = true;
                end = end + 2000;
                var nos = Enumerable.Range(end-1999, end);
                
                lock (noList)
                {
                    nos.AsParallel().ForAll(n => noList.Add(n,n));
                }
                IsLoading = false;
            }
            catch (Exception exception)
            {
               Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GenerateNos",exception,Logger.LogLevel.Error);
            }
        }

        #endregion

        #region protected Methods

        #endregion

        #region Public Methods
        
        public static SequenceNumberGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new SequenceNumberGenerator();
                    }
                }
                return _instance;
            }
        }

        public int GetNextNo()
        {
            try
            {
                if (noList.Count <= 1500)
                    if (!IsLoading)
                        new Thread(GenerateNos).Start();
                var id = noList.First();
                lock (noList)
                {
                    noList.Remove(id.Key);
                }
                return id.Value;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "",exception,Logger.LogLevel.Error);
                return -999;
            }
        }

        #endregion

        #endregion


    }
}
