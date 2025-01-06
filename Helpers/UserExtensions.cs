namespace WebBanLapTop.Helpers
{
    public static class UserExtensions
    {
        public static bool IsUserLoggedIn(this HttpContext context)

        {
            /*
                Context.User?.Identity: Dùng toán tử ?. để tránh lỗi nếu User là null.

                Context.User.Identity.IsAuthenticated: Kiểm tra xem người dùng có được xác thực hay không.
             */
            return context.User?.Identity != null && context.User.Identity.IsAuthenticated;
        }
    }
}
