using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusicManager
{
    public class WaveformVisualizer : Control
    {
        private float[] samples = new float[2048];
        private int writePos = 0;

        public WaveformVisualizer()
        {
            this.DoubleBuffered = true;
        }

        public void AddSamples(float[] buffer, int count)
        {
            for (int i = 0; i < count; i++)
            {
                samples[writePos] = buffer[i];
                writePos = (writePos + 1) % samples.Length;
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (samples == null || samples.Length == 0)
                return;

            var g = e.Graphics;
            int mid = this.Height / 2;
            float widthStep = (float)this.Width / samples.Length;

            using (Pen p = new Pen(Color.DodgerBlue, 1))
            {
                for (int i = 1; i < samples.Length; i++)
                {
                    float x1 = (i - 1) * widthStep;
                    float y1 = mid - samples[(i - 1)] * mid;

                    float x2 = i * widthStep;
                    float y2 = mid - samples[i] * mid;

                    g.DrawLine(p, x1, y1, x2, y2);
                }
            }
        }
    }
}
