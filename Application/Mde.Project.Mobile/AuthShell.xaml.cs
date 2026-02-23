using Mde.Project.Mobile.Pages.Authentication;

namespace Mde.Project.Mobile
{
    public partial class AuthShell : Shell
    {
        public AuthShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(MauiRoutes.AuthSignIn, typeof(SignInPage));
            Routing.RegisterRoute(MauiRoutes.AuthSignUp, typeof(SignUpPage));
            Routing.RegisterRoute(MauiRoutes.AuthForgotPassword, typeof(ForgetPasswordPage));
            Routing.RegisterRoute(MauiRoutes.AuthVerifyCode, typeof(VerifyCodePage));
            Routing.RegisterRoute(MauiRoutes.AuthResetPassword, typeof(ResetPasswordPage));
        }
    }
}
