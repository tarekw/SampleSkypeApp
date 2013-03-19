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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Resources;
using System.IO;

namespace SkypeTest.Engine.Enablers
{
    /// <summary>
    /// A class for handling different kinds of protocols being used to fetch data from the server
    /// </summary>
    public abstract class DataFetcher
    {
        private static object _lock = new object();

        /// <summary>
        /// A factory method for creating the right kind of DataFetcher depending on the protocol requested
        /// </summary>
        /// <param name="protocol">The type of protocol requested</param>
        /// <returns>A DataFetcher instance</returns>
        public static DataFetcher CreateDataFetcher(Protocol protocol)
        {
            lock (_lock)
            {
                DataFetcher instance = null;
                switch (protocol)
                {
                    case Protocol.LOCAL:
                        instance = new LocalDataFetcher();
                        break;
                    case Protocol.REMOTE:
                        instance = new RemoteDataFetcher();
                        break;
                    default:
                        break;
                }
                return instance;
            }
        }

        public String address { get; set; }
        public CancellationToken token { get; set; }

        /// <summary>
        /// A virtual method for fetching data from the server. This method should be overridden by the derived classes
        /// </summary>
        /// <returns>A string representing the returned data</returns>
        public virtual async Task<String> Fetch()
        {
            // default implementation does nothing
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(0)));
            return String.Empty;
        }
    }

    /// <summary>
    /// A DataFetcher subclass for fetching data from local server. This is used mainly for testing purposes.
    /// </summary>
    public class LocalDataFetcher : DataFetcher
    {
        public override async Task<String> Fetch()
        {
            if (String.IsNullOrEmpty(address))
            {
                return String.Empty;
            }

            // wait for few seconds to simulate network activity
            await TaskEx.Delay(TimeSpan.FromSeconds(3));
            return await TaskEx.Run(() => GetLocalData(address));
        }

        private String GetLocalData(String address)
        {
            Uri u = new Uri(address, UriKind.RelativeOrAbsolute);
            StreamResourceInfo sri = Application.GetResourceStream(u);
            StreamReader sr = new StreamReader(sri.Stream);
            string data = sr.ReadToEnd();
            sr.Close();

            if (!String.IsNullOrEmpty(data))
                return data;
            else
                return String.Empty;
        }
    }

    /// <summary>
    /// TODO: This class currently does nothing, since we are doing a simulation.
    /// It needs to be implemented when we want to fetch real data from the live server
    /// </summary>
    public class RemoteDataFetcher : DataFetcher
    {
        public override async Task<String> Fetch()
        {
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(0)));
            return String.Empty;
        }
    }
}
