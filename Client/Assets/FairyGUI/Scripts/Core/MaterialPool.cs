﻿using System.Collections.Generic;
using UnityEngine;

namespace FairyGUI
{
	/// <summary>
	/// 
	/// </summary>
	class MaterialPool
	{
		List<NMaterial> _items;
		List<NMaterial> _blendItems;
		MaterialManager _manager;
		string[] _variants;

		public MaterialPool(MaterialManager manager, string[] variants)
		{
			_manager = manager;
			_variants = variants;
		}

		public NMaterial Get()
		{
			List<NMaterial> items;

			if (_manager.blendMode == BlendMode.Normal)
			{
				if (_items == null)
					_items = new List<NMaterial>();
				items = _items;
			}
			else
			{
				if (_blendItems == null)
					_blendItems = new List<NMaterial>();
				items = _blendItems;
			}

			int cnt = items.Count;
			NMaterial result = null;
			for (int i = 0; i < cnt; i++)
			{
				NMaterial mat = items[i];
				if (mat.frameId == _manager.frameId)
				{
					if (mat.clipId == _manager.clipId && mat.blendMode == _manager.blendMode)
						return mat;
				}
				else if (result == null)
					result = mat;
			}

			if (result != null)
			{
				result.frameId = _manager.frameId;
				result.clipId = _manager.clipId;
				result.blendMode = _manager.blendMode;
			}
			else
			{
				result = _manager.CreateMaterial();
				if (_variants != null)
				{
					foreach (string v in _variants)
						result.EnableKeyword(v);
				}
				result.frameId = _manager.frameId;
				result.clipId = _manager.clipId;
				result.blendMode = _manager.blendMode;
				items.Add(result);
			}

			return result;
		}

		public void Clear()
		{
			if (_items != null)
				_items.Clear();

			if (_blendItems != null)
				_blendItems.Clear();
		}

		public void Dispose()
		{
			if (_items != null)
			{
				if (Application.isPlaying)
				{
					foreach (NMaterial mat in _items)
						Material.Destroy(mat);
				}
				else
				{
					foreach (NMaterial mat in _items)
						Material.DestroyImmediate(mat);
				}
				_items = null;
			}

			if (_blendItems != null)
			{
				if (Application.isPlaying)
				{
					foreach (NMaterial mat in _blendItems)
						Material.Destroy(mat);
				}
				else
				{
					foreach (NMaterial mat in _blendItems)
						Material.DestroyImmediate(mat);
				}
				_blendItems = null;
			}
		}
	}
}