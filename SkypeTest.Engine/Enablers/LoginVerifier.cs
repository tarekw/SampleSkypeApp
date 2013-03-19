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
using SkypeTest.Engine.Models;
using System.Threading.Tasks;

namespace SkypeTest.Engine.Enablers
{
    /// <summary>
    /// A class for verifying the login credentials of a user. It is capable of handling different kinds of protocols for making the verification request.
    /// </summary>
    public abstract class LoginVerifier
    {
        private static object _lock = new object();

        /// <summary>
        /// A factory method for creating the right kind of verifier depending on the protocol requested.
        /// </summary>
        /// <param name="protocol">The type of protocol requested</param>
        /// <returns>A LoginVerifier instance</returns>
        public static LoginVerifier CreateLoginVerifier(Protocol protocol)
        {
            lock (_lock)
            {
                LoginVerifier instance = null;
                switch (protocol)
                {
                    case Protocol.LOCAL:
                        instance = new DummyLoginVerifier();
                        break;
                    case Protocol.REMOTE:
                        instance = new RemoteloginVerifier();
                        break;
                    default:
                        break;
                }
                return instance;
            }
        }

        public Credentials credentials { get; set; }
        public virtual async Task<bool> Verify()
        {
            // default implementation does nothing
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(0)));
            return false;
        }
    }

    /// <summary>
    /// A dummy LoginVerifier for simulating a login request. This class checks the credentials against a predefined
    /// value. It is used mainly for testing purposes.
    /// </summary>
    public class DummyLoginVerifier : LoginVerifier
    {
        public override async Task<bool> Verify()
        {
            if (String.IsNullOrWhiteSpace(credentials.Username) || String.IsNullOrWhiteSpace(credentials.Password))
            {
                return false;
            }

            // wait for few seconds to simulate network activity
            await TaskEx.Delay(TimeSpan.FromSeconds(3));
            return await TaskEx.Run(() => VerifyCreds(credentials));
        }

        private bool VerifyCreds(Credentials credentials)
        {
            // check against a dummy username/password here
            if (credentials.Username == "tarek" && credentials.Password == "tarek")
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// TODO: This class currently does nothing, since we are doing a simulation.
    /// It needs to be implemented when we want to verify real credentials with the live server
    /// </summary>
    public class RemoteloginVerifier : LoginVerifier
    {
        public override async Task<bool> Verify()
        {
            await TaskEx.WhenAny(TaskEx.Delay(TimeSpan.FromSeconds(0)));
            return false;
        }
    }
}
