using System.Net;
using System.Net.Mime;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class XMLController : BaseController<XMLController>
{

    /// <summary>
    /// 保存数据到xml文件
    /// </summary>
    /// <param name="data">数据对象</param>
    /// <param name="file_name">文件名</param>
    public void SavaData(object data, string file_name)
    {
        // 获取储存路径
        string path = Application.persistentDataPath + "/" + data.GetType() + "/" + file_name + ".xml";

        // 创建写入器，序列化并储存文件
        using(StreamWriter writer = new StreamWriter(path))
        {
            XmlSerializer s = new XmlSerializer(data.GetType());
            s.Serialize(writer, data);
        }
    }

    /// <summary>
    /// 从文件中读取内容
    /// </summary>
    /// <param name="type">对象类型</param>
    /// <param name="file_name">文件名</param>
    /// <returns></returns>
    public object LoadData(Type type, string file_name)
    {
        // 在两个路径中分别查找文件
        string path = Application.persistentDataPath + "/" + type.Name + "/" + file_name + ".xml";
        if(!File.Exists(path))
            path = Application.streamingAssetsPath + "/" + type.Name + "/" + file_name + ".xml";
        if(!File.Exists(path))
            return Activator.CreateInstance(type);  // 如未找到则返回一个默认文件

        // 使用指定路径读取文件
        using (StreamReader reader = new StreamReader(path))
        {
            XmlSerializer s = new XmlSerializer(type.GetType());
            s.Deserialize(reader);
        }
        return null;
    }
}
