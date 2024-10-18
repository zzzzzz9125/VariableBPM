using System;
using System.Drawing;
using System.Windows.Forms;
using ScriptPortal.Vegas;

namespace VariableBpm
{
    public sealed partial class VariableBpm : DockableControl
    {
        public Vegas MyVegas;

        public VariableBpm()
            : base("VariableBpm")
        {
            InitializeComponent();
            PersistDockWindowState = true;
        }

        public override DockWindowStyle DefaultDockWindowStyle
        {
            get { return DockWindowStyle.Floating; }
        }

        public override Size DefaultFloatingSize
        {
            get { return new Size(500, 300); }
        }

        protected override void OnLoaded(EventArgs args)
        {
            base.OnLoaded(args);
        }

        protected override void InitLayout()
        {
            base.InitLayout();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)

            base.OnVisibleChanged(e);
        }
    }
}