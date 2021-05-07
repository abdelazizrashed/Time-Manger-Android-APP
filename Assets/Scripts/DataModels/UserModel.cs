

public sealed class UserModel
{
    private static UserModel instance = null;
    private static readonly object padlock = new object();

    public int userID { get; set; }
    public string username { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string accessToken { get; set; }
    public string refreshToken { get; set; }

    UserModel()
    {
        userID = 0;
        username = "username";
        firstName = "first name";
        lastName = "last name";
        email = "email";
        accessToken = "access token";
        refreshToken = "refresh token";
    }

    public static UserModel Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new UserModel();
                    //Todo: set the values of the user.
                }
                return instance;
            }
        }
    }
}
