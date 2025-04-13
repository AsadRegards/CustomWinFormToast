using CustomWinFormToast;
using System.Drawing.Drawing2D;
using Timer = System.Windows.Forms.Timer;

public class ToastForm : Form
{
    private Label messageLabel;
    private Timer lifeTimer;
    private Timer slideTimer;
    private int targetX;
    private int targetY;
    private ToastType toastType;

    private PictureBox iconBox;
    private Form parentForm;

    public ToastForm(string message, ToastType type, Form parent=null)
    {
        toastType = type;
        parentForm = parent;

        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.Width = 300;
        this.Height = 100;
        this.TopMost = true;
        this.ShowInTaskbar = false;
        this.Opacity = 0.95;
        this.BackColor = Color.White;
        this.DoubleBuffered = true;

        ApplyRoundedCorners();

        iconBox = new PictureBox
        {
            Location = new Point(10, 30),
            Size = new Size(40, 40),
            SizeMode = PictureBoxSizeMode.StretchImage
        };

        messageLabel = new Label
        {
            AutoSize = false,
            Location = new Point(60, 20),
            Size = new Size(this.Width - 70, 60),
            Text = message,
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.Black,
            TextAlign = ContentAlignment.MiddleLeft
        };

        this.Controls.Add(iconBox);
        this.Controls.Add(messageLabel);

        SetAppearanceByType(type);

        // 📌 Set position based on parent form or screen
        if (parentForm != null)
        {
            Point parentTopRight = parentForm.PointToScreen(Point.Empty);
            targetX = parentTopRight.X + parentForm.Width - this.Width - 10;
            targetY = parentTopRight.Y + 10;
        }
        else
        {
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            targetX = workingArea.Right - this.Width - 10;
            targetY = workingArea.Top + 10;
        }

        this.Location = new Point(targetX + 100, targetY); // Start a bit right for slide-in

        // Slide-in animation
        slideTimer = new Timer { Interval = 10 };
        slideTimer.Tick += SlideIn;
        slideTimer.Start();

        // Auto-close after 4 seconds
        lifeTimer = new Timer { Interval = 4000 };
        lifeTimer.Tick += (s, e) =>
        {
            slideTimer.Stop();
            slideTimer.Tick -= SlideIn;
            slideTimer.Tick += SlideOut;
            slideTimer.Start();
            lifeTimer.Stop();
        };
        lifeTimer.Start();
    }

    private void SlideIn(object sender, EventArgs e)
    {
        if (this.Left > targetX)
        {
            this.Left -= 10;
        }
        else
        {
            slideTimer.Stop();
        }
    }

    private void SlideOut(object sender, EventArgs e)
    {
        if (this.Left < targetX + 100)
        {
            this.Left += 10;
        }
        else
        {
            slideTimer.Stop();
            this.Close();
        }
    }

    private void SetAppearanceByType(ToastType type)
    {
        switch (type)
        {
            case ToastType.Success:
                this.BackColor = Color.FromArgb(198, 239, 206);
                iconBox.Image = SystemIcons.Information.ToBitmap();
                break;
            case ToastType.Error:
                this.BackColor = Color.FromArgb(255, 199, 206);
                iconBox.Image = SystemIcons.Error.ToBitmap();
                break;
            case ToastType.Warning:
                this.BackColor = Color.FromArgb(255, 235, 156);
                iconBox.Image = SystemIcons.Warning.ToBitmap();
                break;
            case ToastType.Info:
                this.BackColor = Color.FromArgb(219, 238, 244);
                iconBox.Image = SystemIcons.Information.ToBitmap();
                break;
        }
    }

    private void ApplyRoundedCorners()
    {
        var d = 25;
        var path = new GraphicsPath();
        path.StartFigure();
        path.AddArc(new Rectangle(0, 0, d, d), 180, 90);
        path.AddArc(new Rectangle(this.Width - d, 0, d, d), 270, 90);
        path.AddArc(new Rectangle(this.Width - d, this.Height - d, d, d), 0, 90);
        path.AddArc(new Rectangle(0, this.Height - d, d, d), 90, 90);
        path.CloseFigure();
        this.Region = new Region(path);
    }
}
