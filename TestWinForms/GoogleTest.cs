using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoogleMusicAPI;
using System.Net;
using System.Threading.Tasks;

namespace GoogleMusicTest
{
    public partial class GoogleTest : Form
    {
        API api = new API();
        GoogleMusicPlaylists pls;

        public GoogleTest()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            api.Login(tbEmail.Text, tbPass.Text, result =>
            {
                if (result.Data)
                {
                    this.Invoke(new MethodInvoker(() =>
                           {
                               this.Text += " -> Logged in";
                           }));
                }
            });
        }

        void api_OnCreatePlaylistComplete(AddPlaylistResp resp)
        {
            if (resp.Success)
                MessageBox.Show("Created pl");
        }

        private void btnCreatePl_Click(object sender, EventArgs e)
        {
            api.AddPlaylist("Testing", (result) =>
            {

            });
        }

        private void btnFetchSongs_Click(object sender, EventArgs e)
        {
            api.GetAllSongs((result) =>
                {
                    int num = 1;
                    this.Invoke(new MethodInvoker(delegate { lvSongs.BeginUpdate(); }));
                    foreach (var song in result.Data)
                    {
                        var lvi = new ListViewItem();
                        lvi.Text = (num++).ToString();
                        lvi.SubItems.Add(song.Title);
                        lvi.SubItems.Add(song.Artist);
                        lvi.SubItems.Add(song.Album);
                        lvi.SubItems.Add(song.ID);
                        lvi.SubItems.Add(song.Type.ToString());
                        this.Invoke(new MethodInvoker(delegate { lvSongs.Items.Add(lvi); }));
                    }
                    this.Invoke(new MethodInvoker(delegate { lvSongs.EndUpdate(); }));
                });
        }

        private void btnGetPlaylists_Click(object sender, EventArgs e)
        {
            api.GetAllPlaylists(result =>
            {
                this.pls = result.Data;

                this.Invoke(new MethodInvoker(delegate
                {
                    foreach (GoogleMusicPlaylist pl in pls.UserPlaylists)
                    {
                        lbPlaylists.Items.Add(pl.Title);
                    }

                    foreach (GoogleMusicPlaylist pl in pls.InstantMixes)
                    {
                        lbPlaylists.Items.Add(pl.Title);
                    }
                }));
            });
        }

        private void btnSongURL_Click(object sender, EventArgs e)
        {
            string id = lvSongs.SelectedItems[0].SubItems[4].Text;
            api.GetSongURL(id, result =>
                {
                    new WebClient().DownloadFile(result.Data.URL, "C:\\test.mp3");
                });
        }

        private void btnDeletePl_Click(object sender, EventArgs e)
        {
            string id = string.Empty;
            foreach (GoogleMusicPlaylist pl in pls.UserPlaylists)
            {
                if (pl.Title.Equals(lbPlaylists.SelectedItem.ToString()))
                {
                    id = pl.PlaylistID;
                    break;
                }
            }

            api.DeletePlaylist(id, result =>
            {
                if (!String.IsNullOrEmpty(result.Data.ID))
                {
                    MessageBox.Show("Deleted");
                }
            });
        }

        private void btnGetPlaylistSongs_Click(object sender, EventArgs e)
        {
            string id = string.Empty;
            foreach (GoogleMusicPlaylist pl in pls.UserPlaylists)
            {
                if (pl.Title.Equals(lbPlaylists.SelectedItem.ToString()))
                {
                    id = pl.PlaylistID;
                    break;
                }
            }

            api.GetPlaylist(id, result =>
            {
            });
        }
    }
}
