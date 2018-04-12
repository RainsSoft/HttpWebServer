namespace SharpWeb
{
    public interface IHttpFile
    {
        /// <summary>
        /// Gets or sets content type.
        /// </summary>
        string ContentType { get;  }

        /// <summary>
        /// Gets or sets name in form.
        /// </summary>
        string Name { get;  }

        /// <summary>
        /// Gets or sets name original file name
        /// </summary>
        string OriginalFileName { get;  }

        /// <summary>
        /// Gets or sets filename for locally stored file.
        /// </summary>
        string TempFileName { get;  }
    }
}