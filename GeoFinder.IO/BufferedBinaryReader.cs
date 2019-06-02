using System;
using System.IO;

namespace GeoFinder.IO
{
    /// <summary>
    /// Аналог BinaryReader, улучшающая производительность за счет реализации собственных методов чтения без лишних проверок.
    /// </summary>
    public class BufferedBinaryReader : IDisposable
    {
        private readonly Stream _stream;
        /// <summary>
        /// Буфер 
        /// </summary>
        private readonly byte[] _buffer;
        /// <summary>
        /// Размер буфера по умолчанию
        /// </summary>
        private readonly int _bufferSize;
        /// <summary>
        /// Смещение относительно начала буфера до еще не прочитанных байтов
        /// </summary>
        private int _bufferOffset;
        /// <summary>
        /// Количество байт в буфере
        /// </summary>
        private int _numBufferedBytes;

        public BufferedBinaryReader(Stream stream, int bufferSize)
        {
            this._stream = stream;
            this._bufferSize = bufferSize;
            _buffer = new byte[bufferSize];
            _bufferOffset = bufferSize;
        }

        /// <summary>
        /// Количество буферизованных байтов готовых к чтению
        /// </summary>
        public int NumBytesAvailable { get { return Math.Max(0, _numBufferedBytes - _bufferOffset); } }

        /// <summary>
        /// Метод пополнения буфера
        /// </summary>
        /// <returns></returns>
        public bool FillBuffer()
        {
            // Количество непрочитанных байтов
            var numBytesUnread = _bufferSize - _bufferOffset;
            // Количество байтов необходимых для заполнения буфера
            var numBytesToRead = _bufferSize - numBytesUnread;
            // Сбрасываем смещение
            _bufferOffset = 0;
            _numBufferedBytes = numBytesUnread;

            if (numBytesUnread > 0)
            {
                // Переносим непрочитанные байты в начало
                Buffer.BlockCopy(_buffer, numBytesToRead, _buffer, 0, numBytesUnread);
            }
            while (numBytesToRead > 0)
            {
                // Запоняем буфер
                var numBytesRead = _stream.Read(_buffer, numBytesUnread, numBytesToRead);
                if (numBytesRead == 0)
                {
                    return false;
                }
                _numBufferedBytes += numBytesRead;
                numBytesToRead -= numBytesRead;
                numBytesUnread += numBytesRead;
            }
            return true;
        }

        /// <summary>
        /// Возвращает 32-битовое целое число без знака, преобразованное из четырех байтов из буфера
        /// и перемещает текущую позицию в буфере на четыре байта вперед.
        /// </summary>
        public uint ReadUInt32()
        {
            // Логическое сложение байтов. Каждый последующий операнд смещается на байт влево
            uint val = (uint)((int)_buffer[_bufferOffset] | (int)_buffer[_bufferOffset + 1] << 8 |
                              (int)_buffer[_bufferOffset + 2] << 16 | (int)_buffer[_bufferOffset + 3] << 24);
            // Аналог чтения из буфера. Улучшение в производительности не замечено.
            //uint val = BitConverter.ToUInt32(_buffer, _bufferOffset);

            _bufferOffset += 4;
            return val;
        }

        /// <summary>
        /// Возвращает 64-битовое целое число без знака, преобразованное из восьми байтов из буфера
        /// и перемещает текущую позицию в буфере на восемь байтов вперед.
        /// </summary>
        public ulong ReadUInt64()
        {
            // Логическое сложение байтов. Каждый последующий операнд смещается на байт влево
            ulong val = (ulong)((int)_buffer[_bufferOffset] | (int)_buffer[_bufferOffset + 1] << 8 |
                                (int)_buffer[_bufferOffset + 2] << 16 | (int)_buffer[_bufferOffset + 3] << 24 |
                                (int)_buffer[_bufferOffset + 4] << 32 | (int)_buffer[_bufferOffset + 5] << 40 |
                                (int)_buffer[_bufferOffset + 6] << 48 | (int)_buffer[_bufferOffset + 7] << 56);

            //ulong val = BitConverter.ToUInt64(_buffer, _bufferOffset);

            _bufferOffset += 8;
            return val;
        }

        /// <summary>
        /// Возвращает 32-битовое целое число со знаком, преобразованное из четырех байтов из буфера
        /// и перемещает текущую позицию в буфере на четыре байта вперед.
        /// </summary>
        public int ReadInt32()
        {
            // Логическое сложение байтов. Каждый последующий операнд смещается на байт влево
            int val = (int)((int)_buffer[_bufferOffset] | (int)_buffer[_bufferOffset + 1] << 8 |
                            (int)_buffer[_bufferOffset + 2] << 16 | (int)_buffer[_bufferOffset + 3] << 24);

            //int val = BitConverter.ToInt32(_buffer, _bufferOffset);

            _bufferOffset += 4;
            return val;
        }

        /// <summary>
        /// Возвращает число одинарной точности с плавающей запятой, преобразованное из четырех байтов из буфера
        /// и перемещает текущую позицию в буфере на четыре байта вперед.
        /// </summary>
        public float ReadSingle()
        {
            float val = BitConverter.ToSingle(_buffer, _bufferOffset);
            _bufferOffset += 4;
            return val;
        }

        /// <summary>
        /// Считывает байты из буфера и перемещает текущую позицию буфера вперед.
        /// </summary>
        /// <param name="index">Стартовая точка в буфере относительно смещения, начиная с которой считываемые данные возвращаются.</param>
        /// <param name="count">Количество байтов, чтение которых необходимо выполнить.</param>
        /// <returns></returns>
        public byte[] Read(int index, int count)
        {
            byte[] buffer = new byte[count];
            Buffer.BlockCopy(_buffer, _bufferOffset + index, buffer, 0, count);
            _bufferOffset += count;
            return buffer;
        }

        public void Dispose()
        {
            _stream.Close();
        }
    }
}
