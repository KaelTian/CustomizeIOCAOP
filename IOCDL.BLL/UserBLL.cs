
using IOCDL.IBLL;
using IOCDL.IDAL;

namespace IOCDL.BLL
{
    public class UserBLL : IUserBLL
    {
        public IUserDAL _userDAL;

        public UserBLL(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }
        public string Login(string message)
        {
            return $"User BLL message:{_userDAL.Login(message)}";
        }
    }
}
