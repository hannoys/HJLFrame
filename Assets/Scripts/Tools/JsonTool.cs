using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HJLFrame {
    /// <summary>
    /// 一个简单的写入/读取 单个/多个Json对象的工具类 请确保导入了Newtonsoft.Json
    /// </summary>
    public static class JsonTool
    {
        /// <summary>
        /// 读取某个路径下 只有单个json对象的json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>返回单个对象</returns>
        public static T ReadJsonSingle<T>(string path)
        {
            T t = default(T);
            try
            {
                string jsonStr = File.ReadAllText(path);
                t = JsonConvert.DeserializeObject<T>(jsonStr);
                
            }
            catch (Exception e)
            {
                Debug.LogError("读取"+ path + "下的配置失败，错误信息为：" + e);
            }
            return t;
        }
        /// <summary>
        /// 读取某个路径下 有多个json对象的json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>以list返回</returns>
        public static List<T> ReadJsonConfigMul<T>(string path)
        {
            List<T> list = new List<T>();
            try
            {
                string jsonStr = File.ReadAllText(path);
                list = JsonConvert.DeserializeObject<List<T>>(jsonStr);
            }
            catch (Exception e)
            {
                Debug.LogError("读取" + path + "下的配置失败，错误信息为：" + e);
            }
            return list;
        }
        /// <summary>
        /// 往路径覆盖写入单个json对象 File.WriteAllText
        /// </summary>
        /// <typeparam name="T">要序列化的对象</typeparam>
        /// <param name="t"></param>
        /// <param name="path">路径</param>
        public static void WriteJsonSingle<T>(T t,string path)
        {
            string json = JsonConvert.SerializeObject(t, Formatting.Indented);
            File.WriteAllText(path, json);
        }
        /// <summary>
        /// 往路径覆盖写入单个json对象 File.WriteAllText
        /// </summary>
        /// <typeparam name="T">要序列化的对象</typeparam>
        /// <param name="list">要序列化的对象构成List</param>
        /// <param name="path">路径</param>

        public static void WriteJsonMul<T>(List<T> list, string path)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, json);
        }
       
   



 



    }
}
