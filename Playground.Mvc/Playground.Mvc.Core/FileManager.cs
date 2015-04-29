using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Playground.Mvc.Core.Base;
using Playground.Mvc.DataModel;

namespace Playground.Mvc.Core
{
    public class FileManager : BaseManager
    {
        private IQueryable<UploadedFile> Query { get { return Database.Files.AsQueryable().OrderBy(x => x.Id); } }

        public virtual bool Add(byte[] content, string contentType)
        {
            if (content == null) { throw new ArgumentNullException("content"); }
            if (content.Length == 0) { throw new ArgumentException("content.Length == 0"); }
            if (string.IsNullOrWhiteSpace(contentType)) { throw new ArgumentException("contentType IsNullOrWhiteSpace"); }

            Database.Files.Add(new UploadedFile
            {
                Content = content,
                ContentType = contentType
            });

            return (Database.SaveChanges() == 1);
        }

        public virtual bool Add(Stream inputStream, int contentLength, string contentType)
        {
            using (var binaryReader = new BinaryReader(inputStream))
            {
                return Add(binaryReader.ReadBytes(contentLength), contentType);
            }
        }

        public virtual IEnumerable<int> FetchFileId(int skip = 0, int take = 10)
        {
            return Query
                .Select(x => x.Id)
                .Skip(skip)
                .Take(take);
        }

        public virtual UploadedFile GetFileById(int id)
        {
            return Query.SingleOrDefault(x => x.Id == id);
        }
    }
}
