using StUtil.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.IO
{
    public class TempFile : IDisposable
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the file exists; otherwise, <c>false</c>.
        /// </value>
        public bool Exists
        {
            get
            {
                return File.Exists(FileName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public TempFile(string fileName)
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        public TempFile()
            : this(Path.GetTempFileName())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="extension">The extension.</param>
        public TempFile(string path, string extension)
            : this(path, StringGenerator.GenerateString(12), extension)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempFile"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        public TempFile(string path, string name, string extension)
            : this(Utilities.GenerateUniqueFileName(Path.Combine(path, name + "." + extension.TrimStart('.'))))
        {
        }

        /// <summary>
        /// Creates the specified file.
        /// </summary>
        /// <param name="throwIfExists">if set to <c>true</c> an exception will be thrown if the file exists.</param>
        /// <exception cref="System.IO.IOException">File already exists</exception>
        public void Create(bool throwIfExists = false)
        {
            if (Exists)
            {
                if (throwIfExists) throw new System.IO.IOException("File already exists");
            }
            else
            {
                File.Create(FileName).Close();
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        public void Delete()
        {
            File.Delete(FileName);
        }

        /// <summary>
        /// Deletes the temp file
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Deletes the temp file
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Exists)
                {
                    Delete();
                }
            }
        }
    }
}
