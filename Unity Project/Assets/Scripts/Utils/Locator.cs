using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aurorunnner.Utilities
{
	public static class Locator<T> where T : class
	{
		// インスタンスへのグローバルなアクセス先
		public static T Instance { get; private set; }

		// インスタンスを設定
		// singletonと違って自動で設定できないから入れ忘れ注意
		public static void Bind(T instance)
		{
			Instance = instance;
		}

		// インスタンスをnullに
		// 忘れるとメモリリークするから要注意
		public static void Unbind(T instance)
		{
			if (Instance == instance)
			{
				Instance = null;
			}
		}

		// 強制的にインスタンスをnullにしたい時に
		public static void Clear()
		{
			Instance = null;
		}
	}
}
