//
//   		Copyright 2017 KeyleXiao.
//     		Contact : Keyle_xiao@hotmail.com 
//
//     		Licensed under the Apache License, Version 2.0 (the "License");
//     		you may not use this file except in compliance with the License.
//     		You may obtain a copy of the License at
//
//     		http://www.apache.org/licenses/LICENSE-2.0
//
//     		Unless required by applicable law or agreed to in writing, software
//     		distributed under the License is distributed on an "AS IS" BASIS,
//     		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     		See the License for the specific language governing permissions and
//     		limitations under the License.
//

using System;
using System.Collections.Generic;
using SmartDataViewer.Helpers;
using UnityEngine;
using System.IO;

namespace SmartDataViewer
{
    [Serializable]
    public class ConfigBase<T> where T : IModel
    {
        public ConfigBase()
        {
            ConfigList = new List<T>();
        }

        public List<T> ConfigList;

        public virtual T SearchByID(int id)
        {
            for (int i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].ID == id)
                {
                    return ConfigList[i];
                }
            }

            return null;
        }

        public virtual T SearchByNickName(string nickname)
        {
            for (int i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].NickName == nickname)
                {
                    return ConfigList[i];
                }
            }

            return null;
        }


        public virtual void Delete(int id)
        {
            int index = 0;
            for (var i = 0; i < ConfigList.Count; i++)
            {
                if (ConfigList[i].ID != id) continue;
                index = i;
                break;
            }

            ConfigList.RemoveAt(index);
        }
        
        
        
        
        // ----- 只在编辑器状态下使用 ----- 
        #if UNITY_EDITOR
        
        /// <summary>
        /// 从本地删除配置
        /// </summary>
        /// <param name="path"></param>
        public virtual void DeleteFromDisk(string path)
        {
            path = PathMapping.GetInstance().DecodePath(path);

            if (File.Exists(path))
                File.Delete(path);
        }


        /// <summary>
        /// 保存配置到本地
        /// </summary>
        /// <param name="path"></param>
        public virtual void SaveToDisk(string path)
        {
            //Modified by keyle 2016.11.29 缩减配置尺寸
            string content = JsonUtility.ToJson(this, false);

            DeleteFromDisk(path);

            path = PathMapping.GetInstance().DecodePath(path);
            
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(path, content);
        }

        #endif
        // ----- 只在编辑器状态下使用 -----


        
        
        
        
        #region 编辑器状态下资源读取，如果是实际项目中使用还请根据实际情况作出修改

        /// <summary>
        /// 加载配置(静态)
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public static V LoadConfig<V>(string path) where V : new()
        {
            return ConfigLoaderFactory.GetInstance(DataLoaderType.UNITY_JSON).LoadConfig<V>(path);
        }


        public static object LoadConfig(Type t, string path)
        {
            return ConfigLoaderFactory.GetInstance(DataLoaderType.UNITY_JSON).LoadConfig(t,path);
        }

        #endregion
    }
}