using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSlot : MonoBehaviour
{
    GameObject selectParticle;
    PlayerMagic magic;
    public PlayerMagic Magic
    {
        get { return magic; }
    }
    Image magicIcon;
    public Image MagicIcon
    {
        get { return magicIcon; }
    }
    Image cooltimeImg;
    Text magicName;
    public Text MagicName
    {
        get { return magicName; }
    }
    Text countTxt;
    

    public int clickCnt { get; private set; }
    public bool isSelected;    // 해당 마법이 선택되었는지
    bool isReady;       // 마법 사용준비가 되었는지
    public bool IsReady
    {
        get { return isReady; }
    }
    float coolTime;     // 쿨타임

    private void Awake()
    {
        isReady = true;
        isSelected = false;

        magicIcon = transform.Find("SkillImage").GetComponent<Image>();
        cooltimeImg = transform.Find("CooltimeImage").GetComponent<Image>();
        magicName = transform.Find("SkillName").GetComponent<Text>();
        countTxt = transform.Find("CountText").GetComponent<Text>();
        selectParticle = transform.Find("CardglowType02").gameObject;
        selectParticle.gameObject.SetActive(false);
    }

    public void UseMagic(Vector3 direction)
    {
        Magic.ActiveMagic(direction + new Vector3(0, 2, 0));
        countTxt.text = magic.possessionCount.ToString();
        clickCnt++;
        SetState(false);
        StartCoroutine(StartCoolTime());
    }

    public void SetState(bool _State)
    {
        isSelected = _State;
        selectParticle.gameObject.SetActive(_State);
    }

    public void Init(PlayerMagic _Magic)
    {
        magic = _Magic;
        //magic.SetLevelValue(_Magic.level);

        coolTime = magic.magicStat[magic.level].coolTime;
        magicIcon.sprite = magic.icon;
        magicName.text = magic.magicName;
        countTxt.text = magic.possessionCount.ToString();
    }

    public IEnumerator StartCoolTime()
    {
        cooltimeImg.fillAmount = 1f;
        isReady = false;

        while (cooltimeImg.fillAmount > 0)
        {
            cooltimeImg.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
            if (cooltimeImg.fillAmount <= 0)
            {
                isReady = true;
            }
            yield return null;
        }
    }

}
