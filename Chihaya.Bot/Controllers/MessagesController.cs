using Chihaya.Bot.Dialogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;

namespace Chihaya.Bot.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        readonly RootDialog rootDialog;

        public MessagesController(
            RootDialog rootDialog)
        {
            this.rootDialog = rootDialog;
        }

        [HttpPost]
        public async Task Post([FromBody]Activity activity)
        {
            if (activity.GetActivityType() == ActivityTypes.Message)
                await Conversation.SendAsync(activity, () => this.rootDialog);
            else
                await this.HandleSystemMessage(activity);
        }

        private async Task HandleSystemMessage(Activity activity)
        {
        }
    }
}
