using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ControllTeamService {
        public static bool ChangePlayerlRoll() { return true; }
        public static bool SendInvitation() { return true; }
        public static bool MakeOffer() { return true; }
        public static bool AcceptOffer() { return true; }
        public static bool DenyOffer() { return true; }

    }

    public static class UserControlService
    {
        public static bool Register() { return true; }
        public static bool Authorize() { return true; }
        public static bool ChangeSubscription() { return true; }
        public static bool AcceptTeamInvitation() { return true; }
        public static bool AcceptRoomInvitation() { return true; }
        public static bool CheckGameRooms() { return true; }
        public static bool CreateGameRoom() { return true; }
    }
}
