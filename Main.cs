using MelonLoader;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRCUnloader
{
	class Main : MelonMod
	{
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			string unloaderpath = "/storage/emulated/0/Download/VRCUnloader";
			Directory.CreateDirectory(unloaderpath);
			FileInfo[] info = new DirectoryInfo(unloaderpath).GetFiles();
			foreach (FileInfo f in info)
			{
				if (f.Name == sceneName)
				{
					MelonLogger.Msg("suitable world detected");
					string[] file = File.ReadAllText(unloaderpath + "/" + f.Name).Split('\n');
					GameObject[] roots = SceneManager.GetSceneAt(0).GetRootGameObjects();
					foreach (string line in file)
					{
						if (line.Split(':')[0] == "G")
						{
							foreach (Transform target in Object.FindObjectsOfType<Transform>())
                            {
								if (target.name.Contains(line.Split('/')[1]))
								{
									target.gameObject.SetActive(false);
								}
							}
						}
						else if (line.Split(':')[0] != "")
						{
							Transform target = roots[int.Parse(line.Split(':')[0])].transform;
							int counter = 1;
							while (line.Split(':')[counter] != "")
							{
								target = target.GetChild(int.Parse(line.Split(':')[counter]));
								counter++;
							}
							if (line.Split('/')[1] == "*")
							{
								target.gameObject.SetActive(false);
							}
							else
							{
								for (int i = 0; i < target.childCount; i++)
								{
									if (target.GetChild(i).name.Contains(line.Split('/')[1]))
									{
										target.GetChild(i).gameObject.SetActive(false);
									}
								}
							}
						}
					}
					MelonLogger.Msg("world optimized succesfully");
					break;
				}
			}	
		}
	}
}