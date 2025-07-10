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
    /// һ���򵥵�д��/��ȡ ����/���Json����Ĺ����� ��ȷ��������Newtonsoft.Json
    /// </summary>
    public static class JsonTool
    {
        /// <summary>
        /// ��ȡĳ��·���� ֻ�е���json�����json�ļ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>���ص�������</returns>
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
                Debug.LogError("��ȡ"+ path + "�µ�����ʧ�ܣ�������ϢΪ��" + e);
            }
            return t;
        }
        /// <summary>
        /// ��ȡĳ��·���� �ж��json�����json�ļ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>��list����</returns>
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
                Debug.LogError("��ȡ" + path + "�µ�����ʧ�ܣ�������ϢΪ��" + e);
            }
            return list;
        }
        /// <summary>
        /// ��·������д�뵥��json���� File.WriteAllText
        /// </summary>
        /// <typeparam name="T">Ҫ���л��Ķ���</typeparam>
        /// <param name="t"></param>
        /// <param name="path">·��</param>
        public static void WriteJsonSingle<T>(T t,string path)
        {
            string json = JsonConvert.SerializeObject(t, Formatting.Indented);
            File.WriteAllText(path, json);
        }
        /// <summary>
        /// ��·������д�뵥��json���� File.WriteAllText
        /// </summary>
        /// <typeparam name="T">Ҫ���л��Ķ���</typeparam>
        /// <param name="list">Ҫ���л��Ķ��󹹳�List</param>
        /// <param name="path">·��</param>

        public static void WriteJsonMul<T>(List<T> list, string path)
        {
            string json = JsonConvert.SerializeObject(list, Formatting.Indented);
            File.WriteAllText(path, json);
        }
       
   



 



    }
}
