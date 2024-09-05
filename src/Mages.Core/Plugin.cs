namespace Mages.Core;

using System;
using System.Collections.Generic;

/// <summary>
/// Defines the plugin essentials.
/// </summary>
/// <remarks>
/// Creates a new plugin.
/// </remarks>
public class Plugin(IDictionary<String, String> metaData, IDictionary<String, Object> content)
{
    #region Fields

    private readonly IDictionary<String, String> _metaData = metaData;
    private readonly IDictionary<String, Object> _content = content;

    #endregion
    #region ctor

    #endregion

    #region Properties

    /// <summary>
    /// Gets the name of the plugin.
    /// </summary>
    public String Name
    {
        get
        {
            var result = default(String);
            _metaData.TryGetValue("name", out result);
            return result;
        }
    }

    /// <summary>
    /// Gets the plugin's meta data.
    /// </summary>
    public IEnumerable<KeyValuePair<String, String>> MetaData => _metaData;

    /// <summary>
    /// Gets the plugin's content.
    /// </summary>
    public IEnumerable<KeyValuePair<String, Object>> Content => _content;

    #endregion
}
