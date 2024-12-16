// --------------------------------------------------------------------
// <copyright file="StringWriterWithEncoding.cs" company="Ellucian">
//     Copyright 2017 - 2021 Ellucian Company L.P. and its affiliates.
// </copyright>
// --------------------------------------------------------------------
using System;
using System.IO;
using System.Text;

namespace PowerCampus.DataAccess
{
    /// <summary>
    /// StringWriterWithEncoding
    /// </summary>
    /// <seealso cref="System.IO.StringWriter" />
    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding encoding;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        public StringWriterWithEncoding() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        /// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting.</param>
        public StringWriterWithEncoding(IFormatProvider formatProvider) : base(formatProvider) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        /// <param name="sb">The <see cref="T:System.Text.StringBuilder" /> object to write to.</param>
        public StringWriterWithEncoding(StringBuilder sb) : base(sb) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        /// <param name="sb">The <see cref="T:System.Text.StringBuilder" /> object to write to.</param>
        /// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting.</param>
        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider) : base(sb, formatProvider) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        /// <param name="newEncoding">The new encoding.</param>
        public StringWriterWithEncoding(Encoding newEncoding) : base() { encoding = newEncoding; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="newEncoding">The new encoding.</param>
        public StringWriterWithEncoding(IFormatProvider formatProvider, Encoding newEncoding) : base(formatProvider) { encoding = newEncoding; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="newEncoding">The new encoding.</param>
        public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider, Encoding newEncoding) : base(sb, formatProvider) { encoding = newEncoding; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
        /// </summary>
        /// <param name="sb">The sb.</param>
        /// <param name="newEncoding">The new encoding.</param>
        public StringWriterWithEncoding(StringBuilder sb, Encoding newEncoding) : base(sb) { encoding = newEncoding; }

        /// <summary>
        /// Gets the <see cref="T:System.Text.Encoding" /> in which the output is written.
        /// </summary>
        public override Encoding Encoding { get { return encoding ?? base.Encoding; } }
    }
}