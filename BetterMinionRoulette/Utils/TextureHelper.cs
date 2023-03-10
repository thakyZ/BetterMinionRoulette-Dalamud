using System;
using System.Collections.Generic;
using System.Linq;

using Dalamud.Data;

using ImGuiScene;

namespace NekoBoiNick.FFXIV.DalamudPlugin.BetterMinionRoulette.Util;

internal static class TextureHelper {
  private static readonly Dictionary<string, TextureWrap> _loadedTextures = new();
  private static readonly Dictionary<uint, TextureWrap> _loadedIconTextures = new();

  private static DataManager DataManager => Plugin.GetPlugin().DataManager;

  public static nint LoadUldTexture(string name) {
    string path = $"ui/uld/{name}_hr1.tex";
    return LoadTexture(_loadedTextures, path, DataManager.GetImGuiTexture);
  }

  public static nint LoadIconTexture(uint id) {
    return LoadTexture(_loadedIconTextures, id, DataManager.GetImGuiTextureIcon);
  }

  public static void Dispose() {
    var values = _loadedTextures.Values.Concat(_loadedIconTextures.Values).ToList();
    _loadedTextures.Clear();
    _loadedIconTextures.Clear();
    values.ForEach(x => x.Dispose());
  }

  private static nint LoadTexture<TKey>(Dictionary<TKey, TextureWrap> cache, TKey key, Func<TKey, TextureWrap?> loadFunc) where TKey : notnull {
    if (cache.TryGetValue(key, out TextureWrap? texture)) {
      return texture.ImGuiHandle;
    }

    texture = loadFunc(key);
    if (texture is null) {
      return nint.Zero;
    }

    cache[key] = texture;
    return texture.ImGuiHandle;
  }
}
