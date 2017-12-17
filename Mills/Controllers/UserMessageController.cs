using Mills.Models;
using System;

namespace Mills.Controllers
{
    public class UserMessageController
    {
        private BoardViewModel boardViewModel;

        public UserMessageController(BoardViewModel boardViewModel)
        {
            this.boardViewModel = boardViewModel;
        }

        public void SetUserMessage(string message)
        {
            boardViewModel.UserMessage = message;
        }
    }
}
