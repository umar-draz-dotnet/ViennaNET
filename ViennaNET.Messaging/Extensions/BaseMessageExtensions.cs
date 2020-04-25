﻿using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ViennaNET.Messaging.Messages;
using ViennaNET.Utils;

namespace ViennaNET.Messaging.Extensions
{
  /// <summary>
  /// Расширение для работы с сообщением
  /// </summary>
  public static class BaseMessageExtensions
  {
    /// <summary>
    /// Запись документ XML в тело сообщения с применением кодировки
    /// </summary>
    /// <param name="message">Сообщение <see cref="BaseMessage"/></param>
    /// <param name="messageBody">Xml документ <see cref="XDocument"/></param>
    /// <param name="encoding">Кодировка <see cref="Encoding"/></param>
    public static void WriteXml(this TextMessage message, XDocument messageBody, Encoding encoding)
    {
      message.ThrowIfNull(nameof(message));
      messageBody.ThrowIfNull(nameof(messageBody));
      encoding.ThrowIfNull(nameof(encoding));

      using var stream = new MemoryStream();
      using var tw = new XmlTextWriter(stream, encoding);
      messageBody.Save(tw);
      tw.Flush();
      stream.Position = 0;

      using var sr = new StreamReader(stream);
      message.Body = sr.ReadToEnd();
    }

    /// <summary>
    ///  Чтение XML документа из сообщения
    /// </summary>
    /// <param name="message">Сообщение <see cref="BaseMessage"/></param>
    public static XDocument ReadXml(this TextMessage message)
    {
      message.ThrowIfNull(nameof(message));

      using var tr = new StringReader(message.Body);
      return XDocument.Load(tr);
    }
  }
}