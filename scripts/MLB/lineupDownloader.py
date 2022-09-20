from bs4 import BeautifulSoup
import requests
import re

def downloadLineups(date):
    url = f"https://www.mlb.com/starting-lineups/" + date
    page = requests.get(url).text
    doc = BeautifulSoup(page, "html.parser")

    div = doc.find(class_="starting-lineups")
    #print(div)
    items = doc.find_all(class_="starting-lineups__matchup")
    #print(items[0])
    for item in items:
        teams = item.find_all(class_="starting-lineups__team-name--link")
        print(teams[0].text.strip() + " @ " + teams[1].text.strip())

        time = item.find(class_="starting-lineups__game-date-time")
        child = time.find('time')
        print("Game Time is " + child['datetime'])

        pitchers = item.find_all(class_="starting-lineups__pitcher--link")
        pitchers = [x for x in pitchers if len(x.text.strip()) > 1]
        for pitcher in pitchers:
            print(pitcher['href'].split('-')[-1] + "   " + pitcher.text.strip())


        print()
        print("Away Team")
        awayLineupGroup = item.find(class_="starting-lineups__team starting-lineups__team--away")
        awayLineup = awayLineupGroup.find_all(class_="starting-lineups__player--link")
        awayPositions = awayLineupGroup.find_all(class_="starting-lineups__player--position")
        for i in range(0, len(awayLineup)):
            print(awayLineup[i]['href'].split('-')[-1] + "   " + str(i+1) + ". " + awayLineup[i].text.strip() + "   " +  awayPositions[i].text.strip())

        print()
        print("Home Team")
        homeLineupGroup = item.find(class_="starting-lineups__team starting-lineups__team--home")
        homeLineup = homeLineupGroup.find_all(class_="starting-lineups__player--link")
        homePositions = homeLineupGroup.find_all(class_="starting-lineups__player--position")
        for i in range(0, len(homeLineup)):
            print(homeLineup[i]['href'].split('-')[-1] + "   " + str(i+1) + ". " + homeLineup[i].text.strip() + "   " +  homePositions[i].text.strip())

        print()
        print()
    #     parent = item.parent
    #     if parent.name != "a":
    #         continue

    #     link = parent['href']
    #     next_parent = item.find_parent(class_="item-container")
    #     try:
    #         price = next_parent.find(class_="price-current").find("strong").string
    #         items_found[item] = {"price": int(price.replace(",", "")), "link": link}
    #     except:
    #         pass

    # sorted_items = sorted(items_found.items(), key=lambda x: x[1]['price'])

    # for item in sorted_items:
    # 	print(item[0])
    # 	print(f"${item[1]['price']}")
    # 	print(item[1]['link'])
    # 	print("-------------------------------")

#downloadLineups("2021-08-08")
downloadLineups("2022-09-10")