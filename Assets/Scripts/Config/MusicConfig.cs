using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/音效配置")]
public class MusicConfig : ScriptableObject
{
    [Header("房间的整体环境音效")]
    public AudioClip[] amb_horror;
    [Header("幽灵移动音效")]
    public AudioClip[] ghost_movement;
    [Header("幽灵凭依音效")]
    public AudioClip[] ghost_cast;
    [Header("闯入者脚步声")]
    public AudioClip[] human_footstep;
    [Header("椅子")]
    public AudioClip[] wood_movement;
    [Header("书本")]
    public AudioClip[] book_page_turn;
    [Header("灯具")]
    public AudioClip[] bulb_evil_break;
    [Header("画框")]
    public AudioClip[] frame_broken;
    [Header("闯入者逃跑声音")]
    public AudioClip[] human_horror;
    


}
