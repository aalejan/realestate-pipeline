using RealEstatePipeline.Model;

namespace RealEstatePipeline.ViewModels
{
    public class SharedClientViewModel
    {
        // This ViewModel will be used to pass the SharedClient and ClientRegistration objects to the view
        public SharedClient SharedClient { get; set; }
        public ClientRegistration ClientRegistration { get; set; }
    }
}
