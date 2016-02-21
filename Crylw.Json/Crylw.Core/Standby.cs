using System;
namespace Crylw.Core
{
	internal class Standby
	{
		public int Count;
		public string[] Items = new string[8];
		public void Write(string value)
		{
			if (this.Count == this.Items.Length)
			{
				string[] array = new string[this.Items.Length * 2];
				for (int i = 0; i < this.Count; i++)
				{
					array[i] = this.Items[i];
				}
				this.Items = array;
			}
			this.Items[this.Count] = value;
			this.Count++;
		}
		public override string ToString()
		{
			switch (this.Count)
			{
			case 1:
				return this.Items[0];
			case 2:
				return this.Items[0] + this.Items[1];
			case 3:
				return this.Items[0] + this.Items[1] + this.Items[2];
			case 4:
				return this.Items[0] + this.Items[1] + this.Items[2] + this.Items[3];
			default:
			{
				string[] array = new string[this.Count];
				for (int i = 0; i < this.Count; i++)
				{
					array[i] = this.Items[i];
				}
				return string.Concat(array);
			}
			}
		}
	}
}
