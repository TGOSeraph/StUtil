using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace StUtil.Utilities
{
    /// <summary>
    /// Class for serializing and deserializing objects
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// Desialize an object from a file
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize</typeparam>
        /// <param name="filename">The name of the file to load from</param>
        /// <returns>The serialized object converted back to a .NET object</returns>
        public static T BinaryDeSerializeObjectFromFile<T>(string filename)
        {
            T objectToSerialize;
            using (Stream stream = System.IO.File.Open(filename, FileMode.Open))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                objectToSerialize = (T)bFormatter.Deserialize(stream);
                return objectToSerialize;
            }
        }

        /// <summary>
        /// Serialise an object to a binary file
        /// </summary>
        /// <typeparam name="T">The type of object to serialize</typeparam>
        /// <param name="filename">The file to save to</param>
        /// <param name="objectToSerialize">The object to save</param>
        public static void BinarySerializeObjectToFile<T>(string filename, T objectToSerialize)
        {
            using (Stream stream = System.IO.File.Open(filename, FileMode.Create))
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
            }
        }
    }
}