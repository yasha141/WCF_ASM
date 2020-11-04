using System;
using System.Collections.Generic;
using System.Text;

namespace MusicalService
{
    class APILink
    {
        public static string ApiDomain = "https://2-dot-backup-server-002.appspot.com";
        public static string MemberRegisterPath = "/_api/v2/members";
        public static string MemberLoginPath = "/_api/v2/members/authentication";
        public static string MemberInformationPath = "/_api/v2/members/information";
        public static string SongCreatedPath = "/_api/v2/songs";
        public static string SongMyListPath = "/_api/v2/songs/get-mine";
        public static string SongListPath = "/_api/v2/songs";

        public static string ImageGetUploadLink = "/get-upload-token";
    }
}
