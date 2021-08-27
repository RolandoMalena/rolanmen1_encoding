namespace RemoteFileManager.ExtensionMethods
{
    public static class FileSizeExtensionMethods
    {
        public static string Format(this long? bytes)
        {
            if (!bytes.HasValue)
                return "Unknown";

            return bytes.Value.Format();
        }

        public static string Format(this long bytes)
        {
            var kbs = bytes / 1024;
            var mbs = bytes / 1048576;
            var gbs = bytes / 1073741824;

            return $"{bytes} Bytes / {kbs} KBs / {mbs} MBs / {gbs} GBs";
        }
    }
}
