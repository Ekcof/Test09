using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : UnitBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventsBus.Publish(new OnEnterTraderZone { Player = collision.gameObject.GetComponent<UnitPlayer>(), Trader = this });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventsBus.Publish(new OnLeaveTraderZone { Player = collision.gameObject.GetComponent<UnitPlayer>(), Trader = this });
        }
    }
}
