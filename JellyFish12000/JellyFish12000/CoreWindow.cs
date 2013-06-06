using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JellyFish12000
{
	using Rectangle = Microsoft.Xna.Framework.Rectangle;
	using Color = System.Drawing.Color;

	class CoreWindow : Control
	{
        public const int MOVEMENT_DELTA = 3;

		protected Viewport m_Viewport= new Viewport();
		protected Matrix m_View;
		protected Matrix m_Proj;

		protected float m_CameraArc = -135;
		protected float m_CameraRotation = 0;
		protected float m_CameraDistance = 750;

		public CoreWindow() : base()
		{
			m_Viewport.X = 0;
			m_Viewport.Y = 0;
			m_Viewport.Width = ClientSize.Width;
			m_Viewport.Height = ClientSize.Height;
			m_Viewport.MinDepth = 0;
			m_Viewport.MaxDepth = 1;
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			m_Viewport.Width = ClientSize.Width;
			m_Viewport.Height = ClientSize.Height;
			Invalidate();
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			base.Focus();
			base.Invalidate();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			switch (e.KeyCode)
			{
				case Keys.W:
					m_CameraDistance -=MOVEMENT_DELTA;
					break;
				case Keys.S:
                    m_CameraDistance += MOVEMENT_DELTA;
					break;
				case Keys.A:
                    m_CameraRotation -= MOVEMENT_DELTA;
					break;
				case Keys.D:
                    m_CameraRotation += MOVEMENT_DELTA;
					break;
				case Keys.Z:
                    m_CameraArc -= MOVEMENT_DELTA;
					break;
				case Keys.X:
                    m_CameraArc += MOVEMENT_DELTA;
					break;
			}

			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs eventArgs)
		{
			if (DesignMode)
			{
				PaintUsingSystemDrawing(eventArgs.Graphics);
				return;
			}

			GraphicsDevice device = Core.GetDevice();
			if (device == null)
				return;

			bool needsReset = false;
			switch (device.GraphicsDeviceStatus)
			{
				case GraphicsDeviceStatus.NotReset:
					needsReset = true;
					break;
				default:
					PresentationParameters pp = device.PresentationParameters;
					needsReset = (ClientSize.Width > pp.BackBufferWidth) || (ClientSize.Height > pp.BackBufferHeight);
					break;
			}

			if (needsReset)
				Core.ResetDevice(ClientSize.Width, ClientSize.Height);

			try
			{
				device.Clear(Microsoft.Xna.Framework.Color.Black);
				device.Viewport = m_Viewport;

				// Compute camera matrices.
				float aspectRatio = (float)m_Viewport.Width / (float)m_Viewport.Height;
				m_View = Matrix.CreateTranslation(0, -25, 0) *
							  Matrix.CreateRotationZ(MathHelper.ToRadians(m_CameraRotation)) *
							  Matrix.CreateRotationX(MathHelper.ToRadians(m_CameraArc)) *
							  Matrix.CreateLookAt(new Vector3(0, 0, -m_CameraDistance), new Vector3(0, 0, 0), Vector3.Up);

				m_Proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 10000);

				Render(device);

				Rectangle sourceRectangle = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
				device.Present(sourceRectangle, null, this.Handle);
			}
			catch (Exception e)
			{
				//TODO: maybe a log delegate
				e.ToString();
			}
		}

		protected virtual void Render(GraphicsDevice device) {}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//TODO: can use this for painting over the top of our XNA stuff
		}

		protected void PaintUsingSystemDrawing(Graphics graphics)
		{
			graphics.Clear(Color.CornflowerBlue);

			using (Brush brush = new SolidBrush(Color.Black))
			{
				using (StringFormat format = new StringFormat())
				{
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;

					graphics.DrawString("PaintUsingSystemDrawing", Font, brush, ClientRectangle, format);
				}
			}
		}
	}
}
