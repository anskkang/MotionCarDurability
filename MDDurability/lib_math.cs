using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motion.Durability
{
    public class lib_math
    {
		//-------------------------------------------------------------------//
		// matrix operator(include scalar) 3by3 //
		//---------------- Transpose(a[3][3]) = b[3][3]----------------------//
		public static void mattr(double[] a, ref double[] b)
		{
			b[0] = a[0];
			b[1] = a[3];
			b[2] = a[6];
			b[3] = a[1];
			b[4] = a[4];
			b[5] = a[7];
			b[6] = a[2];
			b[7] = a[5];
			b[8] = a[8];
		}

		//-------------------------  a * b = c  -----------------------------//
		public static void scamat(double factor, double[] a, ref double[] b )
		{
			b[0] = factor * a[0];
			b[1] = factor * a[1];
			b[2] = factor * a[2];

			b[3] = factor * a[3];
			b[4] = factor * a[4];
			b[5] = factor * a[5];

			b[6] = factor * a[6];
			b[7] = factor * a[7];
			b[8] = factor * a[8];
		}

		//----------------a[3][3]T * b[3][3] = c[3][3]-----------------------//
		public static void mattrmat(double[] a, double[] b, ref double[] c)
		{
			c[0] = a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
			c[3] = a[0] * b[3] + a[1] * b[4] + a[2] * b[5];
			c[6] = a[0] * b[6] + a[1] * b[7] + a[2] * b[8];
			c[1] = a[3] * b[0] + a[4] * b[1] + a[5] * b[2];
			c[4] = a[3] * b[3] + a[4] * b[4] + a[5] * b[5];
			c[7] = a[3] * b[6] + a[4] * b[7] + a[5] * b[8];
			c[2] = a[6] * b[0] + a[7] * b[1] + a[8] * b[2];
			c[5] = a[6] * b[3] + a[7] * b[4] + a[8] * b[5];
			c[8] = a[6] * b[6] + a[7] * b[7] + a[8] * b[8];
		}

		//------------------a[3][3] * b[3][3] = c[3][3]----------------------//
		public static void matmat(double[] a, double[] b, ref double[] c)
		{
			c[0] = a[0] * b[0] + a[3] * b[1] + a[6] * b[2];
			c[3] = a[0] * b[3] + a[3] * b[4] + a[6] * b[5];
			c[6] = a[0] * b[6] + a[3] * b[7] + a[6] * b[8];
			c[1] = a[1] * b[0] + a[4] * b[1] + a[7] * b[2];
			c[4] = a[1] * b[3] + a[4] * b[4] + a[7] * b[5];
			c[7] = a[1] * b[6] + a[4] * b[7] + a[7] * b[8];
			c[2] = a[2] * b[0] + a[5] * b[1] + a[8] * b[2];
			c[5] = a[2] * b[3] + a[5] * b[4] + a[8] * b[5];
			c[8] = a[2] * b[6] + a[5] * b[7] + a[8] * b[8];
		}

		//----------------a[3][3] * b[3][3]T = d[3][3]-----------------------//
		public static void matmattr(double[] a, double[] b, ref double[] c)
		{
			c[0] = a[0] * b[0] + a[3] * b[3] + a[6] * b[6];
			c[3] = a[0] * b[1] + a[3] * b[4] + a[6] * b[7];
			c[6] = a[0] * b[2] + a[3] * b[5] + a[6] * b[8];
			c[1] = a[1] * b[0] + a[4] * b[3] + a[7] * b[6];
			c[4] = a[1] * b[1] + a[4] * b[4] + a[7] * b[7];
			c[7] = a[1] * b[2] + a[4] * b[5] + a[7] * b[8];
			c[2] = a[2] * b[0] + a[5] * b[3] + a[8] * b[6];
			c[5] = a[2] * b[1] + a[5] * b[4] + a[8] * b[7];
			c[8] = a[2] * b[2] + a[5] * b[5] + a[8] * b[8];
		}

		//--------------a[3][3] * b[3][3] *c[3][3] = d[3][3]-----------------//
		public static void matmatmat(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[9];

			temp1[0] = a[0] * b[0] + a[3] * b[1] + a[6] * b[2];
			temp1[3] = a[0] * b[3] + a[3] * b[4] + a[6] * b[5];
			temp1[6] = a[0] * b[6] + a[3] * b[7] + a[6] * b[8];
			temp1[1] = a[1] * b[0] + a[4] * b[1] + a[7] * b[2];
			temp1[4] = a[1] * b[3] + a[4] * b[4] + a[7] * b[5];
			temp1[7] = a[1] * b[6] + a[4] * b[7] + a[7] * b[8];
			temp1[2] = a[2] * b[0] + a[5] * b[1] + a[8] * b[2];
			temp1[5] = a[2] * b[3] + a[5] * b[4] + a[8] * b[5];
			temp1[8] = a[2] * b[6] + a[5] * b[7] + a[8] * b[8];

			d[0] = temp1[0] * c[0] + temp1[3] * c[1] + temp1[6] * c[2];
			d[3] = temp1[0] * c[3] + temp1[3] * c[4] + temp1[6] * c[5];
			d[6] = temp1[0] * c[6] + temp1[3] * c[7] + temp1[6] * c[8];
			d[1] = temp1[1] * c[0] + temp1[4] * c[1] + temp1[7] * c[2];
			d[4] = temp1[1] * c[3] + temp1[4] * c[4] + temp1[7] * c[5];
			d[7] = temp1[1] * c[6] + temp1[4] * c[7] + temp1[7] * c[8];
			d[2] = temp1[2] * c[0] + temp1[5] * c[1] + temp1[8] * c[2];
			d[5] = temp1[2] * c[3] + temp1[5] * c[4] + temp1[8] * c[5];
			d[8] = temp1[2] * c[6] + temp1[5] * c[7] + temp1[8] * c[8];
		}

		//--------------a[3][3] * b[3][3] *c[3][3]T = d[3][3]----------------//
		public static void matmatmattr(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[9];

			temp1[0] = a[0] * b[0] + a[3] * b[1] + a[6] * b[2];
			temp1[3] = a[0] * b[3] + a[3] * b[4] + a[6] * b[5];
			temp1[6] = a[0] * b[6] + a[3] * b[7] + a[6] * b[8];
			temp1[1] = a[1] * b[0] + a[4] * b[1] + a[7] * b[2];
			temp1[4] = a[1] * b[3] + a[4] * b[4] + a[7] * b[5];
			temp1[7] = a[1] * b[6] + a[4] * b[7] + a[7] * b[8];
			temp1[2] = a[2] * b[0] + a[5] * b[1] + a[8] * b[2];
			temp1[5] = a[2] * b[3] + a[5] * b[4] + a[8] * b[5];
			temp1[8] = a[2] * b[6] + a[5] * b[7] + a[8] * b[8];

			d[0] = temp1[0] * c[0] + temp1[3] * c[3] + temp1[6] * c[6];
			d[3] = temp1[0] * c[1] + temp1[3] * c[4] + temp1[6] * c[7];
			d[6] = temp1[0] * c[2] + temp1[3] * c[5] + temp1[6] * c[8];
			d[1] = temp1[1] * c[0] + temp1[4] * c[3] + temp1[7] * c[6];
			d[4] = temp1[1] * c[1] + temp1[4] * c[4] + temp1[7] * c[7];
			d[7] = temp1[1] * c[2] + temp1[4] * c[5] + temp1[7] * c[8];
			d[2] = temp1[2] * c[0] + temp1[5] * c[3] + temp1[8] * c[6];
			d[5] = temp1[2] * c[1] + temp1[5] * c[4] + temp1[8] * c[7];
			d[8] = temp1[2] * c[2] + temp1[5] * c[5] + temp1[8] * c[8];
		}

		//-------------a[3][3] * b[3][3]T *c[3][3]T = d[3][3]----------------//
		public static void matmattrmattr(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[9];

			temp1[0] = a[0] * b[0] + a[3] * b[3] + a[6] * b[6];
			temp1[3] = a[0] * b[1] + a[3] * b[4] + a[6] * b[7];
			temp1[6] = a[0] * b[2] + a[3] * b[5] + a[6] * b[8];
			temp1[1] = a[1] * b[0] + a[4] * b[3] + a[7] * b[6];
			temp1[4] = a[1] * b[1] + a[4] * b[4] + a[7] * b[7];
			temp1[7] = a[1] * b[2] + a[4] * b[5] + a[7] * b[8];
			temp1[2] = a[2] * b[0] + a[5] * b[3] + a[8] * b[6];
			temp1[5] = a[2] * b[1] + a[5] * b[4] + a[8] * b[7];
			temp1[8] = a[2] * b[2] + a[5] * b[5] + a[8] * b[8];

			d[0] = temp1[0] * c[0] + temp1[3] * c[3] + temp1[6] * c[6];
			d[3] = temp1[0] * c[1] + temp1[3] * c[4] + temp1[6] * c[7];
			d[6] = temp1[0] * c[2] + temp1[3] * c[5] + temp1[6] * c[8];
			d[1] = temp1[1] * c[0] + temp1[4] * c[3] + temp1[7] * c[6];
			d[4] = temp1[1] * c[1] + temp1[4] * c[4] + temp1[7] * c[7];
			d[7] = temp1[1] * c[2] + temp1[4] * c[5] + temp1[7] * c[8];
			d[2] = temp1[2] * c[0] + temp1[5] * c[3] + temp1[8] * c[6];
			d[5] = temp1[2] * c[1] + temp1[5] * c[4] + temp1[8] * c[7];
			d[8] = temp1[2] * c[2] + temp1[5] * c[5] + temp1[8] * c[8];
		}

		//--------------a[3][3]T * b[3][3] *c[3][3] = d[3][3]----------------//
		public static void mattrmatmat(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[9];

			temp1[0] = a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
			temp1[3] = a[0] * b[3] + a[1] * b[4] + a[2] * b[5];
			temp1[6] = a[0] * b[6] + a[1] * b[7] + a[2] * b[8];
			temp1[1] = a[3] * b[0] + a[4] * b[1] + a[5] * b[2];
			temp1[4] = a[3] * b[3] + a[4] * b[4] + a[5] * b[5];
			temp1[7] = a[3] * b[6] + a[4] * b[7] + a[5] * b[8];
			temp1[2] = a[6] * b[0] + a[7] * b[1] + a[8] * b[2];
			temp1[5] = a[6] * b[3] + a[7] * b[4] + a[8] * b[5];
			temp1[8] = a[6] * b[6] + a[7] * b[7] + a[8] * b[8];

			d[0] = temp1[0] * c[0] + temp1[3] * c[1] + temp1[6] * c[2];
			d[3] = temp1[0] * c[3] + temp1[3] * c[4] + temp1[6] * c[5];
			d[6] = temp1[0] * c[6] + temp1[3] * c[7] + temp1[6] * c[8];
			d[1] = temp1[1] * c[0] + temp1[4] * c[1] + temp1[7] * c[2];
			d[4] = temp1[1] * c[3] + temp1[4] * c[4] + temp1[7] * c[5];
			d[7] = temp1[1] * c[6] + temp1[4] * c[7] + temp1[7] * c[8];
			d[2] = temp1[2] * c[0] + temp1[5] * c[1] + temp1[8] * c[2];
			d[5] = temp1[2] * c[3] + temp1[5] * c[4] + temp1[8] * c[5];
			d[8] = temp1[2] * c[6] + temp1[5] * c[7] + temp1[8] * c[8];
		}

		//--------------a[3][3]T * b[3][3] * c[3][3]T = d[3][3]----------------//
		public static void mattrmatmattr(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[9];

			temp1[0] = a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
			temp1[3] = a[0] * b[3] + a[1] * b[4] + a[2] * b[5];
			temp1[6] = a[0] * b[6] + a[1] * b[7] + a[2] * b[8];
			temp1[1] = a[3] * b[0] + a[4] * b[1] + a[5] * b[2];
			temp1[4] = a[3] * b[3] + a[4] * b[4] + a[5] * b[5];
			temp1[7] = a[3] * b[6] + a[4] * b[7] + a[5] * b[8];
			temp1[2] = a[6] * b[0] + a[7] * b[1] + a[8] * b[2];
			temp1[5] = a[6] * b[3] + a[7] * b[4] + a[8] * b[5];
			temp1[8] = a[6] * b[6] + a[7] * b[7] + a[8] * b[8];

			d[0] = temp1[0] * c[0] + temp1[3] * c[3] + temp1[6] * c[6];
			d[3] = temp1[0] * c[1] + temp1[3] * c[4] + temp1[6] * c[7];
			d[6] = temp1[0] * c[2] + temp1[3] * c[5] + temp1[6] * c[8];
			d[1] = temp1[1] * c[0] + temp1[4] * c[3] + temp1[7] * c[6];
			d[4] = temp1[1] * c[1] + temp1[4] * c[4] + temp1[7] * c[7];
			d[7] = temp1[1] * c[2] + temp1[4] * c[5] + temp1[7] * c[8];
			d[2] = temp1[2] * c[0] + temp1[5] * c[3] + temp1[8] * c[6];
			d[5] = temp1[2] * c[1] + temp1[5] * c[4] + temp1[8] * c[7];
			d[8] = temp1[2] * c[2] + temp1[5] * c[5] + temp1[8] * c[8];
		}

		//--------------a[3][3] * b[3][3]T *c[3][3] = d[3][3]----------------//
		public static void matmattrmat(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[9];

			temp1[0] = a[0] * b[0] + a[3] * b[3] + a[6] * b[6];
			temp1[3] = a[0] * b[1] + a[3] * b[4] + a[6] * b[7];
			temp1[6] = a[0] * b[2] + a[3] * b[5] + a[6] * b[8];
			temp1[1] = a[1] * b[0] + a[4] * b[3] + a[7] * b[6];
			temp1[4] = a[1] * b[1] + a[4] * b[4] + a[7] * b[7];
			temp1[7] = a[1] * b[2] + a[4] * b[5] + a[7] * b[8];
			temp1[2] = a[2] * b[0] + a[5] * b[3] + a[8] * b[6];
			temp1[5] = a[2] * b[1] + a[5] * b[4] + a[8] * b[7];
			temp1[8] = a[2] * b[2] + a[5] * b[5] + a[8] * b[8];

			d[0] = temp1[0] * c[0] + temp1[3] * c[1] + temp1[6] * c[2];
			d[3] = temp1[0] * c[3] + temp1[3] * c[4] + temp1[6] * c[5];
			d[6] = temp1[0] * c[6] + temp1[3] * c[7] + temp1[6] * c[8];
			d[1] = temp1[1] * c[0] + temp1[4] * c[1] + temp1[7] * c[2];
			d[4] = temp1[1] * c[3] + temp1[4] * c[4] + temp1[7] * c[5];
			d[7] = temp1[1] * c[6] + temp1[4] * c[7] + temp1[7] * c[8];
			d[2] = temp1[2] * c[0] + temp1[5] * c[1] + temp1[8] * c[2];
			d[5] = temp1[2] * c[3] + temp1[5] * c[4] + temp1[8] * c[5];
			d[8] = temp1[2] * c[6] + temp1[5] * c[7] + temp1[8] * c[8];
		}

		//-------------------------------------------------------------------//
		// matrix and vector operator(include scalar) 3by3&3by1 //
		//----------------------a[3][3] * b[3] = c[3]------------------------//
		public static void matvec(double[] a, double[] b, ref double[] c)
		{
			c[0] = a[0] * b[0] + a[3] * b[1] + a[6] * b[2];
			c[1] = a[1] * b[0] + a[4] * b[1] + a[7] * b[2];
			c[2] = a[2] * b[0] + a[5] * b[1] + a[8] * b[2];
		}

		//---------------------a[3][3]T * b[3] = c[3]------------------------//
		public static void mattrvec(double[] a, double[] b, ref double[] c)
		{
			c[0] = a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
			c[1] = a[3] * b[0] + a[4] * b[1] + a[5] * b[2];
			c[2] = a[6] * b[0] + a[7] * b[1] + a[8] * b[2];
		}

		//----------------------a[3]T * b[3][3] = c[3]-----------------------//
		public static void vectrmat(double[] a, double[] b, ref double[] c)
		{
			c[0] = a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
			c[1] = a[0] * b[3] + a[1] * b[4] + a[2] * b[5];
			c[2] = a[0] * b[6] + a[1] * b[7] + a[2] * b[8];
		}

		//--------------a[3][3] * b[3][3] * c[3] = d[3][3]-------------------//
		public static void matmatvec(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[3];

			temp1[0] = b[0] * c[0] + b[3] * c[1] + b[6] * c[2];
			temp1[1] = b[1] * c[0] + b[4] * c[1] + b[7] * c[2];
			temp1[2] = b[2] * c[0] + b[5] * c[1] + b[8] * c[2];

			d[0] = a[0] * temp1[0] + a[3] * temp1[1] + a[6] * temp1[2];
			d[1] = a[1] * temp1[0] + a[4] * temp1[1] + a[7] * temp1[2];
			d[2] = a[2] * temp1[0] + a[5] * temp1[1] + a[8] * temp1[2];
		}

		//--------------a[3][3]T * b[3][3] * c[3] = d[3][3]-------------------//
		public static void mattrmatvec(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[3];

			temp1[0] = b[0] * c[0] + b[3] * c[1] + b[6] * c[2];
			temp1[1] = b[1] * c[0] + b[4] * c[1] + b[7] * c[2];
			temp1[2] = b[2] * c[0] + b[5] * c[1] + b[8] * c[2];

			d[0] = a[0] * temp1[0] + a[1] * temp1[1] + a[2] * temp1[2];
			d[1] = a[3] * temp1[0] + a[4] * temp1[1] + a[5] * temp1[2];
			d[2] = a[6] * temp1[0] + a[7] * temp1[1] + a[8] * temp1[2];
		}

		//------------a[3][3] * b[3][3]T * c[3] = d[3]-----------------------//
		public static void matmattrvec(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[3];

			temp1[0] = b[0] * c[0] + b[1] * c[1] + b[2] * c[2];
			temp1[1] = b[3] * c[0] + b[4] * c[1] + b[5] * c[2];
			temp1[2] = b[6] * c[0] + b[7] * c[1] + b[8] * c[2];

			d[0] = a[0] * temp1[0] + a[3] * temp1[1] + a[6] * temp1[2];
			d[1] = a[1] * temp1[0] + a[4] * temp1[1] + a[7] * temp1[2];
			d[2] = a[2] * temp1[0] + a[5] * temp1[1] + a[8] * temp1[2];
		}

		//------------a[3][3]T * b[3][3]T * c[3] = d[3]---------------------//
		public static void mattrmattrvec(double[] a, double[] b, double[] c, ref double[] d)
		{
			double[] temp1 = new double[3];

			temp1[0] = b[0] * c[0] + b[1] * c[1] + b[2] * c[2];
			temp1[1] = b[3] * c[0] + b[4] * c[1] + b[5] * c[2];
			temp1[2] = b[6] * c[0] + b[7] * c[1] + b[8] * c[2];

			d[0] = a[0] * temp1[0] + a[1] * temp1[1] + a[2] * temp1[2];
			d[1] = a[3] * temp1[0] + a[4] * temp1[1] + a[5] * temp1[2];
			d[2] = a[6] * temp1[0] + a[7] * temp1[1] + a[8] * temp1[2];
		}

		public static bool Calculate_ROLL_PITCH_YAW(IList<double[]> markerinfo, ref List<double[]> lst_ZYX, int nStartPosition)
        {
			int i, j, k;
			double[] A = new double[9];
			double[] zyx = new double[3] { 0.0, 0.0, 0.0 };

			j = 0;
			foreach(double[] arr in markerinfo)
            {
				for (i = 0; i < 9; i++)
					A[i] = arr[3 + i];

				if (false == Calculate_ROLL_PITCH_YAW(A, ref zyx))
					return false;

				for (k = 0; k < 3; k++)
					lst_ZYX[j][nStartPosition + k] = zyx[k];

			}


			return true;
        }

		public static bool Calculate_ROLL_PITCH_YAW(double[] A, ref double[] ZYX)
		{
			// ZYX[0] : Roll
			// ZYX[1] : Pitch
			// ZYX[2] : Yaw
			const double errtol = 1.0e-10;
			double rtmp = 0.0;

			rtmp = A[0] * A[0] + A[1] * A[1];
			rtmp = Math.Sqrt(rtmp);

			ZYX[1] = Math.Atan2(-A[2], rtmp);

			rtmp = Math.Abs(Math.Cos(ZYX[1]));
			if (rtmp < errtol)
			{
				ZYX[0] = 0.0;
				ZYX[2] = Math.Atan2(-A[3], A[4]);
			}
			else
			{
				ZYX[2] = Math.Atan2(A[1], A[0]);
				ZYX[0] = Math.Atan2(A[5], A[8]);
			}


			return true;
		}
	}
}
