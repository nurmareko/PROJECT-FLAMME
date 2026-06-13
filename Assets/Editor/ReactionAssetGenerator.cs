using System.IO;
using UnityEditor;
using UnityEngine;

public static class ReactionAssetGenerator
{
    private const string FolderPath = "Assets/Reactions";

    [MenuItem("Fullcard/Generate Reaction Assets")]
    public static void GenerateReactionAssets()
    {
        EnsureFolder();

        int created = 0;
        foreach (var seed in Seeds())
        {
            string path = $"{FolderPath}/{seed.resultName}.asset";
            if (AssetDatabase.LoadAssetAtPath<ReactionData>(path) != null)
            {
                Debug.Log($"Reaction asset already exists, skipping: {path}");
                continue;
            }

            var asset = ScriptableObject.CreateInstance<ReactionData>();
            asset.cardA = seed.cardA;
            asset.cardB = seed.cardB;
            asset.perubahan = seed.perubahan;
            asset.kalor = seed.kalor;
            asset.effectPrefab = null;
            asset.resultName = seed.resultName;
            asset.explanation = seed.explanation;
            asset.everydayExample = seed.everydayExample;

            AssetDatabase.CreateAsset(asset, path);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Generated {created} new reaction assets in {FolderPath}. Expected total reaction definitions: 6.");
    }

    private static void EnsureFolder()
    {
        if (AssetDatabase.IsValidFolder(FolderPath)) return;

        string parent = Path.GetDirectoryName(FolderPath)?.Replace("\\", "/");
        string folderName = Path.GetFileName(FolderPath);

        if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(folderName))
        {
            Debug.LogError($"Invalid reaction folder path: {FolderPath}");
            return;
        }

        AssetDatabase.CreateFolder(parent, folderName);
    }

    private static ReactionSeed[] Seeds()
    {
        return new[]
        {
            new ReactionSeed(
                ElementType.Panas,
                ElementType.Es,
                Perubahan.Mencair,
                KalorDir.Menerima,
                "Mencair",
                "Es batu di gelas teh hangat.",
                "Es menerima kalor dari panas. Partikelnya bergetar makin cepat sampai ikatannya melonggar - es padat berubah menjadi air cair."),
            new ReactionSeed(
                ElementType.Panas,
                ElementType.Air,
                Perubahan.Menguap,
                KalorDir.Menerima,
                "Menguap",
                "Air mendidih di panci, jemuran mengering.",
                "Air menerima kalor dan partikelnya bergerak makin cepat hingga lepas ke udara menjadi uap."),
            new ReactionSeed(
                ElementType.Panas,
                ElementType.KapurBarus,
                Perubahan.Menyublim,
                KalorDir.Menerima,
                "Menyublim",
                "Kapur barus di lemari lama-lama habis.",
                "Kapur barus menerima kalor dan berubah langsung dari padat menjadi gas tanpa mencair dulu."),
            new ReactionSeed(
                ElementType.Dingin,
                ElementType.Air,
                Perubahan.Membeku,
                KalorDir.Melepas,
                "Membeku",
                "Air di freezer menjadi es batu.",
                "Air melepas kalor ke udara dingin. Partikelnya melambat dan terkunci rapat - air cair menjadi es padat."),
            new ReactionSeed(
                ElementType.Dingin,
                ElementType.Uap,
                Perubahan.Mengembun,
                KalorDir.Melepas,
                "Mengembun",
                "Embun di gelas es, embun pagi di daun.",
                "Uap air melepas kalor saat menyentuh permukaan dingin, lalu berubah menjadi titik-titik air."),
            new ReactionSeed(
                ElementType.Es,
                ElementType.Uap,
                Perubahan.Mengkristal,
                KalorDir.Melepas,
                "Mengkristal",
                "Bunga es di dinding freezer.",
                "Uap air yang menyentuh permukaan sangat dingin melepas kalor dan langsung menjadi kristal es tanpa menjadi air dulu.")
        };
    }

    private readonly struct ReactionSeed
    {
        public readonly ElementType cardA;
        public readonly ElementType cardB;
        public readonly Perubahan perubahan;
        public readonly KalorDir kalor;
        public readonly string resultName;
        public readonly string everydayExample;
        public readonly string explanation;

        public ReactionSeed(
            ElementType cardA,
            ElementType cardB,
            Perubahan perubahan,
            KalorDir kalor,
            string resultName,
            string everydayExample,
            string explanation)
        {
            this.cardA = cardA;
            this.cardB = cardB;
            this.perubahan = perubahan;
            this.kalor = kalor;
            this.resultName = resultName;
            this.everydayExample = everydayExample;
            this.explanation = explanation;
        }
    }
}
