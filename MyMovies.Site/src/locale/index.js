import engEnum from "./engEnum";
import engStr from "./engStr";
import rusEnum from "./rusEnum";
import rusStr from "./rusStr";
import ukrEnum from "./ukrEnum";
import ukrStr from "./ukrStr";

const locale = {
  en: {
    ...engEnum,
    ...engStr
  },
  ru: {
    ...rusEnum,
    ...rusStr
  },
  uk: {
    ...ukrEnum,
    ...ukrStr
  }
};
export default locale;