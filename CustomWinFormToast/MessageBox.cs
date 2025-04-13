namespace CustomWinFormToast
{
    public static class MessageBox
    {
        private static void ShowToast(string message, ToastType type = ToastType.Info, Form parentForm = null)
        {
            ToastForm toast = new ToastForm(message, type, parentForm);
            toast.Show();
        }

        public static void ShowInfoMessage(string message, Form parentForm = null)
        {
            ShowToast(message, ToastType.Info, parentForm);
        }

        public static void ShowErrorMessage(string message, Form parentForm = null)
        {
            ShowToast(message, ToastType.Error, parentForm);
        }

        public static void ShowSuccessMessage(string message, Form parentForm = null)
        {
            ShowToast(message, ToastType.Success, parentForm);
        }

        public static void ShowWarningMessage(string message, Form parentForm = null)
        {
            ShowToast(message, ToastType.Warning, parentForm);
        }
    }
}
