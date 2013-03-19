using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using SkypeTest.Engine.Models;

namespace SkypeTest.Engine.Enablers
{
    /// <summary>
    /// The supported protocols for communicating with the server
    /// </summary>
    public enum Protocol
    {
        LOCAL,
        REMOTE
        // add other specific protocols here
    };
 
    /// <summary>
    /// A utility engine for asynchronously fetching data and verifying user credentials
    /// </summary>
    public class BackendEnabler
    {
        private static object _lock = new object();
        private static BackendEnabler _backendEnabler = null;

        /// <summary>
        /// The default BackendEnabler instance.
        /// </summary>
        public static BackendEnabler Default
        {
            get
            {
                lock (_lock)
                {
                    if (_backendEnabler == null)
                    {
                        _backendEnabler = new BackendEnabler();
                    }
                    return _backendEnabler;
                }
            }
            set
            {
                lock (_lock)
                {
                    _backendEnabler = value;
                }
            }
        }

        /// <summary>
        /// A method for asynchronously requesting different types of data from the server.
        /// </summary>
        /// <typeparam name="T">The generic type of data being requested. This is defined by the model classes that encapsulates the format of the data being requested</typeparam>
        /// <param name="dataFetcher">An object that handles the underlying protocol being used to fetch the data</param>
        /// <returns>A deserialized object modelling the type of data returned</returns>
        public async Task<T> StartDataRequest<T>(DataFetcher dataFetcher)
        {
            try
            {
                var response = await dataFetcher.Fetch();
                return await TaskEx.Run(() => ParseResponse<T>(response));
            }
            catch (Exception) { return default(T); }
        }

        private T ParseResponse<T>(string data)
        {
            if (data == null || data.Length == 0)
            {
                return default(T);
            }
            try
            {
                var result = JsonConvert.DeserializeObject<T>(data);
                return result;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Asynchronously initiates a verification request with the backend servers.
        /// </summary>
        /// <param name="verifier">An object that handles the underlying protocol used for communicating with the servers</param>
        /// <returns>A boolean value depending on whether the has been verified or not</returns>
        public async Task<bool> StartLoginVerification(LoginVerifier verifier)
        {
            try
            {
                var response = await verifier.Verify();
                return response;
            }
            catch (Exception) { return false; }
        }
    }
}
