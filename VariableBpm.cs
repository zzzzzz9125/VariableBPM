#if !Sony
using ScriptPortal.Vegas;
#else
using Sony.Vegas;
#endif

using System;
using System.Drawing;

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
            get { return new Size(500, 450); }
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