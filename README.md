# Oligopoly
Oligopoly is a CLI game written in C# that allows players to buy and sell shares of various in-game companies. The value of these shares is affected by in-game events. This is not a professional project, so don't expect high quality code or regular updates.

## How to add your own company to the game
1. Open Data.xml file.
2. Find Companies element.
3. Copy the following template and paste it below the last Company element in the file:
      ```
      <Company id="COMPANY_ID">
        <Name>COMPANY_NAME</Name>
        <Ticker>COMPANY_TICKER</Ticker>
        <Industry>COMPANY_INDUSTRY</Industry>
        <SharePrice>COMPANY_SHARE_PRICE</SharePrice>
        <Description>COMPANY_DESCRIPTION</Description>
      </Company>
      ```
4. Replace COMPANY_ID, COMPANY_NAME, COMPANY_TICKER, COMPANY_INDUSTRY, COMPANY_SHARE_PRICE, and COMPANY_DESCRIPTION with the appropriate values for your new company.
Company id value can be anything, make it so that it is easier for you to navigate in the xml file. Make sure the Ticker attribute value is unique and does not match any other company Ticker values in the file. Industry and Description CANNOT be null or whitespace. SharePrice can be ONLY positive.
5. Save the changes to the XML file.

## How to add your own event to the game
1. Open Data.xml file.
2. Find Events element.
3. Copy the following template and paste it below the last Company element in the file:
      ```
      <Event id="EVENT_ID">
            <Effect>EVENT_EFFECT</Effect>
            <Target>EVENT_TARGET</Target>
            <Type>EVENT_TYPE</Type>
            <Title>EVENT_TITLE</Title>
            <Content>EVENT_CONTENT</Content>
          </Event>
      ```
4. Replace EVENT_ID, EVENT_EFFECT, EVENT_TARGET, EVENT_TYPE, EVENT_TITLE and EVENT_CONTENT with the appropriate values for your new event. Event id value can be anything, make it so that it is easier for you to navigate in the xml file. Effect can be ONLY positive. Make sure that Target attribute matches the Ticker attribute of the company whose price is need to be changed. Type attribute value can be ONLY Positive or Negative. Title and Content CANNOT be null or whitespace.
5. Save the changes to the XML file.

## License
This project is licensed under the [MIT License](https://github.com/Fuinny/Oligopoly/blob/master/LICENSE.md).
