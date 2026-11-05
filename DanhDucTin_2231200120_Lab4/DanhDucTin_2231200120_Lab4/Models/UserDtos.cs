namespace Lab4
{
    // DTO dùng cho chức năng Đăng ký tài khoản
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // DTO dùng cho chức năng Kích hoạt tài khoản
    public class ActivateDto
    {
        public string Code { get; set; }
    }
}