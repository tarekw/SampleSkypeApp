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
using SkypeTest.Engine.Enablers;
using SkypeTest.Engine.Models;

namespace SkypeTest.ViewModels
{
    /// <summary>
    /// The viewmodel responsible for requesting user verification from the backend engine.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        public bool Verified { get; private set; }

        /// <summary>
        /// Asynchronously verifies the credentials of the user depending on the type protocol 
        /// </summary>
        /// <param name="credentials">User credentials to be verified</param>
        public async void VerifyCrednetials(Credentials credentials)
        {
            NotifyStateChanged(CurrentState.Loading);   // this should show the waiting state

            LoginVerifier verifier = null;
#if USING_LIVE_DATA
                // TODO: write the code for requesting live content using a REST api
#else
            verifier = LoginVerifier.CreateLoginVerifier(Protocol.LOCAL);
#endif
            verifier.credentials = credentials;
            Verified = await BackendEnabler.Default.StartLoginVerification(verifier);

            if (Verified)
                NotifyStateChanged(CurrentState.EndLoading);   // this should show the waiting state
            else
                NotifyStateChanged(CurrentState.Error);   // this should show the waiting state
        }
    }
}
