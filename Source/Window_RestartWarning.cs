using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;


namespace QualityEverything
{
    public class Window_RestartWarning : Window
    {
        static string label;

        public Window_RestartWarning(string text)
        {
            this.layer = WindowLayer.Dialog;
            this.forcePause = false;
            this.absorbInputAroundWindow = false;
            this.soundAppear = SoundDefOf.DialogBoxAppear;
            this.soundClose = SoundDefOf.Click;
            this.preventCameraMotion = true;
            this.onlyOneOfTypeAllowed = true;
            this.resizeable = false;
            this.closeOnCancel = false;
            this.closeOnAccept = false;
            this.draggable = false;
            this.doCloseButton = true;
            label = text;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(600f, 150f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect rect = inRect.ContractedBy(10f);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperCenter;
            Widgets.Label(inRect.ContractedBy(10f), label);
            Text.Anchor = TextAnchor.UpperLeft;
        }

    }
}
